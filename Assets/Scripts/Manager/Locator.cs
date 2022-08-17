using UnityEngine;
using UnityEngine.UI;

namespace Pinball
{
    public class Locator : Singleton<Locator>
    {
        [SerializeField]
        private Puller _playerPuller;
        [SerializeField]
        private Camera _playerCamera;
        [SerializeField]
        private Image _staminaBar;

        public Puller GetPuller { get { return _playerPuller; } }
        public Camera GetCamera { get { return _playerCamera; } }
        public Image GetStamina { get { return _staminaBar; } }
    }
}