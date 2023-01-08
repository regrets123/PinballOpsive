using UnityEngine;

namespace Pinball
{
    public class PointSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pointPrefab;
        [SerializeField]
        private LayerMask _collideWithLayer;

        private PullTarget SpawnPoint(Vector3 spawnPoint)
        {
            GameObject newPoint = Instantiate(_pointPrefab, spawnPoint, Quaternion.identity, transform);
            return newPoint.GetComponent<PullTarget>();
        }

    }
}