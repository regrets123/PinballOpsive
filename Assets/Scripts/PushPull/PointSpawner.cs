using UnityEngine;

namespace Pinball
{
    public class PointSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pointPrefab;
        [SerializeField]
        private LayerMask _collideWithMask;
        public PullTarget SpawnBelow()
        {
            Ray beam = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(beam, out RaycastHit hit, 1000f, _collideWithMask))
            {
                return SpawnPoint(hit.point);
            }
            return null;
        }

        public PullTarget SpawnAhead(Vector3 velDir)
        {
            Ray beam = new Ray(transform.position, velDir);
            if (Physics.Raycast(beam, out RaycastHit hit, 1000f, _collideWithMask))
            {
                return SpawnPoint(hit.point);
            }
            return null;
        }

        private PullTarget SpawnPoint(Vector3 spawnPoint)
        {
            GameObject newPoint = Instantiate(_pointPrefab, spawnPoint, Quaternion.identity, transform);
            return newPoint.GetComponent<PullTarget>();
        }

    }
}