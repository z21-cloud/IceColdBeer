using System;
using IceColdBeer.Core;
using UnityEngine;

namespace IceColdBeer.Factories
{
    public class CoinFactory: IFactory<Coin>
    {
        private readonly Coin _prefab;
        private readonly Transform _parent;
        private readonly CoinPool _pool;

        public CoinFactory(Coin prefab, CoinPool pool, Transform parent = null)
        {
            _prefab = prefab;
            _pool = pool;
            _parent = parent;
        }

        public Coin Create()
        {
            var coin = GameObject.Instantiate(_prefab, _parent);
            coin.Initialize(_pool);
            return coin;
        }
    }
}
