using System;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class Coin : MonoBehaviour, IPickable
    {
        [SerializeField] private int value = 1;
        private CoinPool _coinPool;

        public int Value => value;

        public void Initialize(CoinPool coinPool)
        {
            _coinPool = coinPool;
        }

        public void PickUp()
        {
            Debug.Log("Coin picked up");
            
            Destroy(gameObject);
            //_coinPool.ReleaseCoin(this);
        }
    }
}
