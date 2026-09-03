using IceColdBeer.Core;
using IceColdBeer.Factories;
using UnityEngine;

namespace IceColdBeer.Pools
{
    public class WinHolePool : MonoBehaviour
    {
        [SerializeField] private WinHole hole;
        [SerializeField] private int poolSize = 1;
        [SerializeField] private Transform parent;

        private ObjectPooling<WinHole> pool;

        private void Awake()
        {
            var factory = new WinHoleFactory(hole, parent);
            pool = new ObjectPooling<WinHole>(factory, poolSize);

            Debug.Log($"[WinHolePool] Initialized with {poolSize} win holes.");
        }

        public WinHole GetHole()
        {
            WinHole hole = pool.Get();
            if(hole != null) return hole;

            Debug.LogWarning($"[WinHolePool] Win Hole Pool is empty, return null!");
            return null;
        }

        public void ReleaseHole(WinHole hole)
        {
            pool.Release(hole);
        }
    }
}
