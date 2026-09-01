using System.Collections.Generic;
using IceColdBeer.Core;
using IceColdBeer.Factories;
using UnityEngine;

namespace IceColdBeer.Pools
{
    public class HolePool : MonoBehaviour
    {
        [SerializeField] private LoseHole hole;
        [SerializeField] private int poolSize;
        [SerializeField] private Transform parent;
        // [SerializeField] private List<Transform> spawnPoints;

        private ObjectPooling<LoseHole> pool;

        private void Awake()
        {
            var factory = new LoseHoleFactory(hole, this, parent);
            pool = new ObjectPooling<LoseHole>(factory, poolSize);

            /*if(spawnPoints.Count == 0)
            {
                Debug.LogWarning($"[HolePool] No spawn points!");
            }

            foreach(var spawnPoint in spawnPoints)
            {
                var hole = GetHole();
                if(hole != null)
                {
                    hole.transform.position = spawnPoint.position;
                }
            }*/

            Debug.Log($"[HolePool] Initialized with {poolSize} holes.");
        }

        public LoseHole GetHole()
        {
            LoseHole hole = pool.Get();
            if(hole != null) return hole;

            Debug.LogWarning($"[HolePool] Hole Pool is empty, return null!");
            return null;
        }

        public void ReleaseHole(LoseHole hole)
        {
            pool.Release(hole);
        }
    }
}
