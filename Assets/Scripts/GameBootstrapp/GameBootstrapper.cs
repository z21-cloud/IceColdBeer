using IceColdBeer.Core;
using IceColdBeer.Level;
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
        
        [Header("Pools")]
        [SerializeField] private WinHolePool _winHolePool;
        [SerializeField] private CoinPool _coinPool;
        [SerializeField] private HolePool _loseHolePool;
        [SerializeField] private LevelGenerator _levelGenerator;

        private void Awake()
        {
            _ballPickable.Initailize(_scoreManager);
            _platformMover.Initialize(_inputManager);
            _uiManager.Intitialize(_scoreManager);
            
            _levelGenerator.Initailize(_loseHolePool, _coinPool, _winHolePool, _scoreManager);
        }
    }
}
