using IceColdBeer.Core;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Factories
{
    public class WinHoleFactory : IFactory<WinHole>
    {
        private readonly WinHole _prefab;
        private readonly Transform _parent;
        private readonly IScoreCounter _scoreCounter;
        private readonly ICoinCounter _coinCounter;

        public WinHoleFactory(WinHole prefab, IScoreCounter scoreCounter, ICoinCounter coinCounter, Transform parent = null)
        {
            _prefab = prefab;
            _scoreCounter = scoreCounter;
            _coinCounter = coinCounter;
            _parent = parent;
        }

        public WinHole Create()
        {
            var winHole = GameObject.Instantiate(_prefab, _parent);

            winHole.Initialize(_scoreCounter, _coinCounter);

            return winHole;
        }
    }
}
