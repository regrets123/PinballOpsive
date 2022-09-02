using UnityEngine;

namespace Pinball
{
    public class TrackPlayerRotateWithCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;
        [SerializeField]
        private Transform _camera;

        private void Update()
        {
            transform.position = _player.position;
            Quaternion fixedCamera = _camera.rotation;
            fixedCamera.z = 0;
            fixedCamera.x = 0;
            transform.rotation = fixedCamera;
        }
    }
}