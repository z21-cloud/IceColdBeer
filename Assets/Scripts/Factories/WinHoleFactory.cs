using IceColdBeer.Core;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Factories
{
    public class WinHoleFactory : IFactory<WinHole>
    {
        private readonly WinHole _prefab;
        private readonly Transform _parent;

        public WinHoleFactory(WinHole prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public WinHole Create()
        {
            var winHole = GameObject.Instantiate(_prefab, _parent);

            return winHole;
        }
    }
}
