using UnityEngine.Assertions;
using UnityEngine;

namespace Pinball
{
    public class PullTarget : MonoBehaviour
    {
        [SerializeField]
        private float _maxPullDist = 300f;
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
            if (_myRenderer.isVisible && Vector3.Distance(transform.position, _myPuller.transform.position) < _maxPullDist)
            {
                _myPuller.AddMe(this);
            }
            else
            {
                _myPuller.RemoveMe(this);
                TargetMe(false);
            }
        }

        public bool IsInRange(Vector3 playerPosition, float pullRange)
        {
            float currentDistance = Vector3.Distance(transform.position, playerPosition);
            if (currentDistance <= pullRange)
            {
                _myRenderer.material.color = Color.green;
                return true;
            }
            _myRenderer.material.color = Color.blue;
            return false;
        }

        public void TargetMe(bool isTarget)
        {
            if(isTarget)
            {
                _myRenderer.material.color = Color.blue;
            }
            else
            {
                _myRenderer.material.color = Color.red;
            }
        }
    }
}