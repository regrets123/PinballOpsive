using UnityEngine;
using UnityEngine.UI;

namespace Pinball
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField]
        private float _maxStamina;
        [SerializeField]
        private float _currentStamina;
        [SerializeField]
        private float _costPerUse;
        [SerializeField]
        private float _regenCooldown;
        [SerializeField]
        private float _regenRate;
        private float _currentCooldown;
        private Image _image;

        private void Awake()
        {
            _image = Locator.Instance.GetStamina;
        }

        public bool ConsumeStamina(float percentageMod)
        {
            if(_currentStamina > _costPerUse)
            {
                _currentStamina -= _costPerUse * percentageMod;
                UpdateBarVisual();
                RefreshCooldown();
                return true;
            }
            return false;
        }

        private void FixedUpdate()
        {
            if(_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
            else
            {
                RegenStamina();
            }
        }

        private void RegenStamina()
        {
            if(_currentStamina < _maxStamina)
            {
                _currentStamina += _regenRate;
                if (_currentStamina > _maxStamina)
                {
                    _currentStamina = _maxStamina;
                }
                UpdateBarVisual();
            }
        }

        private void RefreshCooldown()
        {
            _currentCooldown = _regenCooldown;
        }

        private void UpdateBarVisual()
        {
            _image.fillAmount = _currentStamina / _maxStamina;
        }
    }
}