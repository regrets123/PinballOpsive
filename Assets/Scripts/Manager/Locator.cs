using UnityEngine;

namespace Pinball
{
    public class Locator : Singleton<Locator>
    {
        [SerializeField]
        private Puller _playerPuller;
        [SerializeField]
        private Camera _playerCamera;

        public Puller GetPuller { get { return _playerPuller; } }
        public Camera GetCamera { get { return _playerCamera; } }
    }
}