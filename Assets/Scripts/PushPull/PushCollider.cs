using UnityEngine;
using System.Collections.Generic;

namespace Pinball
{
    public class PushCollider : MonoBehaviour
    {
        [SerializeField]
        Pusher _push;
        [SerializeField]
        private bool _forward;
        [SerializeField]
        private List<Collider> _insideBox;
        private LayerMask _pullable;

        private void Start()
        {
            _pullable = LayerMask.GetMask("Pullable");
            if (_push != null)
            {
                int index = 0;
                if(!_forward)
                {
                    index = 1;
                }
                _push.AddPushCollider(this, index);
            }
        }

        public Collider[] GetTargets()
        {
            Collider[] toReturn = _insideBox.ToArray();
            return toReturn;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Pullable"))
            {
                _insideBox.Add(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Pullable"))
            {
                _insideBox.Remove(other);
            }
        }
    }
}