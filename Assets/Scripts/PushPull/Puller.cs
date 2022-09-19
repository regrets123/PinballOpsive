using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Opsive.UltimateCharacterController.Character;
using System.Collections.Generic;

namespace Pinball
{
    [RequireComponent(typeof(PlayerInput),typeof(UltimateCharacterLocomotion))]
    public class Puller : MonoBehaviour
    {
        [SerializeField]
        private Camera _playerCam;
        [SerializeField]
        private float _hitRadius;
        [SerializeField]
        private float _maxRange;
        [SerializeField]
        private float _pullRange;
        [SerializeField]
        private float _powerModifier;
        [SerializeField]
        private float _rtThreshhold;
        [SerializeField]
        private float _yTargetOffset = 3f;

        public float GetPullRange { get { return _pullRange; } }
        [SerializeField]
        private LayerMask _targetLayer;
        [SerializeField]
        private LineRenderer _pullRenderer;
        private PullTarget _currentTarget;
        private UltimateCharacterLocomotion _locomotion; 
        private List<PullTarget> _insideCollider;
        private bool _isInRange;
        private Gamepad _pad;

        private void Start()
        {
            _insideCollider = new List<PullTarget>();
            _locomotion = GetComponent<UltimateCharacterLocomotion>();
            _pad = Gamepad.current;
            Assert.IsNotNull(_pullRenderer);
            Assert.IsNotNull(_pad);
            Assert.IsNotNull(_playerCam);
            Assert.IsNotNull(_locomotion);
            _pullRenderer.material = new Material(Shader.Find("Sprites/Default"));

        }

        private void Update()
        {       
            if(_insideCollider.Count > 0)
            {
                _currentTarget = ClosestToCenter();
            }
            if(_currentTarget != null)
            {
                DrawLine();
                _isInRange = _currentTarget.IsInRange(transform.position, _pullRange);
                if (_pad != null)
                {
                    CheckPull();
                }
            }
        }

        private void DrawLine()
        {
            Vector3[] linePos = new Vector3[2];
            linePos[0] = _currentTarget.transform.position;
            linePos[1] = transform.position;
            _pullRenderer.positionCount = linePos.Length;
            _pullRenderer.SetPositions(linePos);
        }

        private void CheckPull()
        {
            float rtValue = _pad.rightTrigger.ReadValue();
            if(rtValue > _rtThreshhold && _isInRange)
            {
                Pull(rtValue);
            }
        }

        private void Pull(float inputStr)
        {
            Vector3 raisedTarget = _currentTarget.transform.position + new Vector3(0, _yTargetOffset, 0);
            Vector3 targetDir = raisedTarget - transform.position;
            Vector3 applyMag = targetDir.normalized * GetPowerModDistanceCapped() * inputStr;
            _locomotion.AddForce(applyMag, 1);
        }

        private float GetPowerModDistanceCapped()
        {
            float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);
            float capMod = _maxRange/ distance;
            capMod *= _powerModifier;
            return capMod;
        }


        public void AddMe(PullTarget me)
        {
            if (!_insideCollider.Contains(me))
            {
                _insideCollider.Add(me);
            }
        }

        public void RemoveMe(PullTarget me)
        {
            _insideCollider.Remove(me);
        }

        private PullTarget ClosestToCenter()
        {
            PullTarget closest;
            float distance = Mathf.Infinity;
            float half = 0.5f;
            int savedIndex = 0;     
            Vector2 screenCenter = new Vector2(_playerCam.pixelWidth * half, _playerCam.pixelHeight * half);
            for (int i = 0; i < _insideCollider.Count; i++)
            {
                Vector2 screenPoint = _playerCam.WorldToScreenPoint(_insideCollider[i].transform.position);
                float newDist = Vector2.Distance(screenCenter, screenPoint);
                if (newDist < distance)
                {
                    distance = newDist;
                    savedIndex = i;
                }
            }
            closest = _insideCollider[savedIndex];
            return closest;
        }

        private bool TryFindTargetAhead()
        {
            Vector3 startDir = _playerCam.transform.forward;
            RaycastHit hits;
            PullTarget newTar;
            if (Physics.SphereCast(_playerCam.transform.position, _hitRadius, startDir, out hits, _maxRange, _targetLayer)
                && hits.transform.TryGetComponent(out newTar))
            {
                _currentTarget = newTar;
                return true;
            }
            return false;
        }
    }
}