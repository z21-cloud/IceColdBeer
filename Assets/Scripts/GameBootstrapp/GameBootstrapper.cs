using IceColdBeer.Core;
using IceColdBeer.PlayerInput;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.GameStarter
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PlatformMover _platformMover;
        [SerializeField] private BallPickable _ballPickable;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private WinHole _winHole;
        [SerializeField] private CoinPool _coinPool;

        private void Awake()
        {
            _ballPickable.Initailize(_scoreManager);
            _platformMover.Initialize(_inputManager);
            _uiManager.Intitialize(_scoreManager);
            _winHole.Initialize(_scoreManager, _coinPool);
        }
    }
}
