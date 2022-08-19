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
        private float _ltThreshhold;


        [SerializeField]
        private LayerMask _targetLayer;
        private PullTarget _currentTarget;
        private UltimateCharacterLocomotion _locomotion;
        private Gamepad _pad;

        private void Start()
        {
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
            //Make this method recursiv in the future to save performance. It would then have a fraction of max range at start and gradually increase to max range. 
            LayerMask mask = LayerMask.NameToLayer("Pullable");
            Collider[] pushers = Physics.OverlapSphere(transform.position, _maxRange, mask, QueryTriggerInteraction.Collide);        
            float dist = Mathf.Infinity;
            int minIndex = -1;
            for (int i = 0; i < pushers.Length; i++)
            {
                float newDist = Vector3.Distance(transform.position, pushers[i].transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    minIndex = i;
                }
            } 
            if(minIndex < 0)
            {
                return null;
            }
            return pushers[minIndex].GetComponent<PullTarget>();
        }

        private void CheckPush()
        {
            float ltValue = _pad.leftTrigger.ReadValue();
            if (ltValue > _ltThreshhold && _staminaRef.ConsumeStamina(ltValue))
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
