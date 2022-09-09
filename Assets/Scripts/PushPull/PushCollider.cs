using UnityEngine;
using System.Collections.Generic;

namespace Pinball
{
    public class PushCollider : MonoBehaviour
    {
        public enum ColliderDir { Below, Forward, Back }
        [SerializeField]
        private ColliderDir _direction;
        [SerializeField]
        Pusher _push;
        [SerializeField]
        private List<Collider> _insideBox;

        public ColliderDir GetDirection { get { return _direction; } }

        private void Start()
        {
            if (_push != null)
            {
                _push.AddPushCollider(this, (int)_direction);
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