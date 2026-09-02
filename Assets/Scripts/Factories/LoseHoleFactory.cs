using IceColdBeer.Core;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Factories
{
    public class LoseHoleFactory : IFactory<LoseHole>
    {
        private readonly LoseHole _prefab;
        private readonly Transform _parent;

        public LoseHoleFactory(LoseHole prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public LoseHole Create()
        {
            var hole = GameObject.Instantiate(_prefab, _parent);
            // hole doesn't need to be initialized with the pool because it doesn't have any methods that require it. It just needs to be instantiated and placed in the scene.
            return hole;
        }
    }
}
