using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Opsive.UltimateCharacterController.Character;

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
        private LayerMask _targetLayer;
        private PullTarget _currentTarget;
        private UltimateCharacterLocomotion _locomotion;
        private bool _isInRange;
        private List<PullTarget> _insideCollider;

        private void Start()
        {
            _insideCollider = new List<PullTarget>();
            _locomotion = GetComponent<UltimateCharacterLocomotion>();
            Assert.IsNotNull(_playerCam);
            Assert.IsNotNull(_locomotion);
        }

        private void Update()
        {
            if(_insideCollider.Count > 0)
            {
                _currentTarget = ClosestToCenter();
            }
            if(_currentTarget != null)
            {
                _currentTarget.TargetMe(true);
                _isInRange = _currentTarget.IsInRange(transform.position, _pullRange);         
            }              
        }

        public void OnFire1(InputAction.CallbackContext callback)
        {
            if (_isInRange)
            {
                Vector3 targetDir = _currentTarget.transform.position - transform.position;
                Vector3 applyMag = targetDir.normalized * _powerModifier;
                _locomotion.AddForce(applyMag, 5);
            }
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
                _insideCollider[i].TargetMe(false);
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
                if(_currentTarget != null)
                {
                    _currentTarget.TargetMe(false);
                }
                _currentTarget = newTar;
                return true;
            }
            return false;
        }
    }
}