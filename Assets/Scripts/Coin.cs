using System;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class Coin : MonoBehaviour, IPickable
    {
        private CoinPool _coinPool;
        public void Initialize(CoinPool coinPool)
        {
            _coinPool = coinPool;
        }

        public void PickUp()
        {
            Debug.Log("Coin picked up");
            _coinPool.ReleaseCoin(this);
        }
    }
}
