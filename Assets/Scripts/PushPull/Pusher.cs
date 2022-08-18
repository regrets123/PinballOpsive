using Opsive.UltimateCharacterController.Character;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

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
        private float _ltThreshhold;


        [SerializeField]
        private LayerMask _targetLayer;
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
            Assert.IsNotNull(_pad);
            Assert.IsNotNull(_playerCam);
            Assert.IsNotNull(_locomotion);
        }

        private void Update()
        {
            _currentTarget = FindPushTarget();
            if (_currentTarget != null)
            {
                _currentTarget.TargetMe(true);
                if (_pad != null)
                {
                    CheckPush();
                }
            }
        }

        private PullTarget FindPushTarget()
        {
            return null;
        }

        private void CheckPush()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue > _ltThreshhold && _staminaRef.ConsumeStamina(ltValue))
            {
                Push(ltValue);
            }
        }

        private void Push(float pushMod)
        {

        }
    }
}
