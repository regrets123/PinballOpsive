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
        private Stamina _staminaRef;
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
        private PullTarget _currentTarget;
        private UltimateCharacterLocomotion _locomotion;
        private Gamepad _pad;
        private LineRenderer _pushLine;

        private void Awake()
        {
            _colliders = new PushCollider[2];    
        }

        private void Start()
        {
            _locomotion = GetComponent<UltimateCharacterLocomotion>();
            _pad = Gamepad.current;
            _pushLine = GetComponent<LineRenderer>();
            _pushLine.material = new Material(Shader.Find("Sprites/Default"));
            Assert.IsNotNull(_pad);
            Assert.IsNotNull(_playerCam);
            Assert.IsNotNull(_locomotion);
        }

        private void Update()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue < _ltThreshold)
            {
                _currentTarget = FindPushTargetColliders();
            }
            if (_currentTarget != null)
            {
                DrawLine();
                _currentTarget.TargetMe(true);
                if (_pad != null)
                {
                    CheckPush();
                }
            }
        }

        public void AddPushCollider(PushCollider collider, int index)
        {
            if (_colliders != null)
            {
                _colliders[index] = collider;
            }
        }

        private void DrawLine()
        {
            Vector3[] linePos = new Vector3[3];
            linePos[0] = _currentTarget.transform.position;
            linePos[1] = transform.position;
            Vector3 direction = linePos[1] - linePos[0];
            linePos[2] = direction.normalized * 2f + transform.position;
            _pushLine.positionCount = linePos.Length;
            _pushLine.SetPositions(linePos);           
        }


        private PullTarget FindPushTargetColliders()
        {
            Vector2 lStick = _pad.leftStick.ReadValue();
            if(lStick.x > _lStickThreshold)
            {
                if(lStick.x < 0)
                {
                    return FindClosets(_colliders[0].GetTargets());
                }
                else
                {
                    return FindClosets(_colliders[1].GetTargets());
                }
            }
            else
            {
                PullTarget forward = FindClosets(_colliders[0].GetTargets());
                PullTarget backward = FindClosets(_colliders[1].GetTargets());
                if(forward != null && backward != null)
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


        private PullTarget FindPushTargetSphere()
        {
            //Make this method recursiv in the future to save performance. It would then have a fraction of max range at start and gradually increase to max range. 
            LayerMask mask = LayerMask.GetMask("Pullable");
            Collider[] pushers = Physics.OverlapSphere(transform.position, _maxRange, mask, QueryTriggerInteraction.Collide);
            Debug.Log("pushTargetArrSize is " +pushers.Length);
            return FindClosets(pushers);
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

        private void CheckPush()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue > _ltThreshold && _staminaRef.ConsumeStamina(ltValue))
            {
                Push(ltValue);
            }
        }

        private void Push(float inputStr)
        {
            Vector3 targetDir = _currentTarget.transform.position - transform.position;
            Vector3 applyMag = targetDir.normalized * _powerModifier * inputStr;
            _locomotion.AddForce(applyMag *-1, 1);
        }
    }
}
