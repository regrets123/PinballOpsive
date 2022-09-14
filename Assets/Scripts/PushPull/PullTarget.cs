using UnityEngine.Assertions;
using UnityEngine;

namespace Pinball
{
    public class PullTarget : MonoBehaviour
    {
        private Renderer _myRenderer;
        private Puller _myPuller;

        private void Start()
        {
            _myRenderer = GetComponent<Renderer>();
            Assert.IsNotNull(_myRenderer);
            _myPuller = Locator.Instance.GetPuller;
        }

        private void Update()
        {
            if (_myRenderer.isVisible && Vector3.Distance(transform.position, _myPuller.transform.position) < _myPuller.GetPullRange)
            {
                _myPuller.AddMe(this);
            }
            else
            {
                _myPuller.RemoveMe(this);
            }
        }

        public bool IsInRange(Vector3 playerPosition, float pullRange)
        {
            float currentDistance = Vector3.Distance(transform.position, playerPosition);
            if (currentDistance <= pullRange)
            {
                return true;
            }
            return false;
        }

    }
}