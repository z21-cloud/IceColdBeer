using IceColdBeer.Core;
using IceColdBeer.PlayerInput;
using UnityEngine;

namespace IceColdBeer.GameStarter
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PlatformMover _platformMover;
        [SerializeField] private BallPickable _ballPickable;
        [SerializeField] private ScoreManager _scoreManager;

        private void Awake()
        {
            _ballPickable.Initailize(_scoreManager);
            _platformMover.Initialize(_inputManager);
        }
    }
}
