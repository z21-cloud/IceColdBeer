using IceColdBeer.Core;
using IceColdBeer.Factories;
using UnityEngine;

namespace IceColdBeer.Pools
{
    public class CoinPool : MonoBehaviour
    {
        [SerializeField] private Coin coin;
        [SerializeField] private int poolSize;
        [SerializeField] private Transform parent;
    
        private ObjectPooling<Coin> pool;

        private void Awake() 
        {
            var factory = new CoinFactory(coin, this, parent);
            pool = new ObjectPooling<Coin>(factory, poolSize);
        }

        public Coin GetCoin()
        {
            Coin coin = pool.Get();
            if(coin != null) return coin;
    
            Debug.LogWarning($"[CoinPool] Coin Pool is empty, return null!");
            return null;
        }

        public void ReleaseCoin(Coin coin)
        {
            pool.Release(coin);
        }
    
    }
}
