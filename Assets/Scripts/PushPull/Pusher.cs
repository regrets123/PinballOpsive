using Opsive.UltimateCharacterController.Character;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Pinball
{
    [RequireComponent(typeof(PlayerInput), typeof(UltimateCharacterLocomotion))]
    public class Pusher : MonoBehaviour
    {
        [SerializeField]
        private Camera _playerCam;
        [SerializeField]
        private float _hitRadius;
        [SerializeField]
        private float _maxRange;
        [SerializeField]
        private float _pushRange;
        [SerializeField]
        private float _powerModifier;
        [SerializeField]
        private float _ltThreshold;
        [SerializeField]
        private float _lStickThreshold;
        [SerializeField]
        private PushCollider[] _colliders;

        [SerializeField]
        private LayerMask _targetLayer;
        [SerializeField]
        private LineRenderer _pushRenderer;
        private PullTarget _currentTarget;
        private UltimateCharacterLocomotion _locomotion;
        private Gamepad _pad;
     
        private Vector3 previousPosition;

        private void Awake()
        {
            _colliders = new PushCollider[3];    
        }

        private void Start()
        {
            _locomotion = GetComponent<UltimateCharacterLocomotion>();
            _pad = Gamepad.current;
            _pushRenderer.material = new Material(Shader.Find("Sprites/Default"));
            previousPosition = Vector3.negativeInfinity;
            Assert.IsNotNull(_pad);
            Assert.IsNotNull(_playerCam);
            Assert.IsNotNull(_locomotion);
            Assert.IsNotNull(_pushRenderer);
        }

        private void Update()
        {
            CheckLtTrigger();        
            if (_currentTarget != null)
            {
                DrawLine();
                if (_pad != null)
                {
                    CheckPush();
                }
            }
            previousPosition = transform.position;
        }

        public void AddPushCollider(PushCollider collider, int index)
        {
            if (_colliders != null)
            {
                _colliders[index] = collider;
            }
        }

        private void CheckLtTrigger()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue < _ltThreshold && !FindPushBelow())
            {
                Vector3 currDir = CheckVelocityDirection();
                if (currDir != Vector3.negativeInfinity)
                {
                    PullTarget newtar = CheckStickAngle(currDir);
                    if (newtar != null)
                        _currentTarget = newtar;
                }
                else
                {
                    PullTarget newtar = FindPushTargetColliders();
                    if (newtar != null)
                        _currentTarget = newtar;
                }
            }
        }

        private Vector3 CheckVelocityDirection()
        {
            Vector3 toReturn = Vector3.negativeInfinity;
            if (previousPosition != Vector3.negativeInfinity && previousPosition != transform.position)
            {
                Vector3 currentDirection = previousPosition - transform.position;
                toReturn = currentDirection.normalized;
            }
            return toReturn;
        }

        private void DrawLine()
        {
            Vector3[] linePos = new Vector3[3];
            linePos[0] = _currentTarget.transform.position;
            linePos[1] = transform.position;
            Vector3 direction = linePos[1] - linePos[0];
            linePos[2] = direction.normalized * 2f + transform.position;
            _pushRenderer.positionCount = linePos.Length;
            _pushRenderer.SetPositions(linePos);           
        }

        private bool FindPushBelow()
        {
            Collider[] below = _colliders[(int)PushCollider.ColliderDir.Below].GetTargets();
            if (below.Length < 1)
            {
                return false;
            }
            else
            {
                _currentTarget = FindClosets(below);
                return true;
            }
        }

        private PullTarget FindPushTargetColliders()
        {
            Vector2 lStick = _pad.leftStick.ReadValue();
            if (Mathf.Abs(lStick.y) > _lStickThreshold)
            {
                if (lStick.y < 0)
                {
                    return FindClosets(_colliders[(int)PushCollider.ColliderDir.Forward].GetTargets());
                }
                else
                {
                    return FindClosets(_colliders[(int)PushCollider.ColliderDir.Back].GetTargets());
                }
            }
            else
            {
                PullTarget forward = FindClosets(_colliders[(int)PushCollider.ColliderDir.Forward].GetTargets());
                PullTarget backward = FindClosets(_colliders[(int)PushCollider.ColliderDir.Back].GetTargets());
                if (forward != null && backward != null)
                {
                    float forwardDist = Vector3.Distance(forward.transform.position, transform.position);
                    float backwardDist = Vector3.Distance(backward.transform.position, transform.position);
                    return forwardDist < backwardDist ? forward : backward;
                }
                if (forward != null)
                {
                    return forward;
                }
                if (backward != null)
                {
                    return backward;
                }
            }
            return null;
        }

    private PullTarget CheckStickAngle(Vector3 currentDirection)
        {
            Vector2 lStick = _pad.leftStick.ReadValue();
            if (Mathf.Abs(lStick.y) > _lStickThreshold)
            {
                if (lStick.y < 0)
                {
                    return FindClosestAngle(_colliders[1].GetTargets(), currentDirection);
                }
                else
                {
                    return FindClosestAngle(_colliders[2].GetTargets(), currentDirection);
                }
            }
            return null;
        }


        private PullTarget FindClosestAngle(Collider[] targets, Vector3 currentDir)
        {
            float dist = Mathf.Infinity;
            int minIndex = -1;
            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 newDir = targets[i].transform.position - transform.position;
                float newAngle = Vector3.Angle(currentDir, newDir.normalized);
                if (newAngle < dist)
                {
                    dist = newAngle;
                    minIndex = i;
                }
            }
            if (minIndex < 0)
            {
                return null;
            }
            return targets[minIndex].GetComponent<PullTarget>();
        }

        private PullTarget FindClosets(Collider[] targets)
        {
            float dist = Mathf.Infinity;
            int minIndex = -1;
            for (int i = 0; i < targets.Length; i++)
            {
                float newDist = Vector3.Distance(transform.position, targets[i].transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    minIndex = i;
                }
            }
            if (minIndex < 0)
            {
                return null;
            }
            return targets[minIndex].GetComponent<PullTarget>();
        }
        private float GetPowerModDistanceCapped()
        {
            float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);
            float capMod = _maxRange /distance;
            capMod *= _powerModifier;
            return capMod;
        }

        private void CheckPush()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue > _ltThreshold)
            {
                Push(ltValue);
            }
        }

        private void Push(float inputStr)
        {
            Vector3 targetDir = _currentTarget.transform.position - transform.position;
            Vector3 applyMag = targetDir.normalized * GetPowerModDistanceCapped() * inputStr;
            _locomotion.AddForce(applyMag *-1, 1);
        }
    }
}
