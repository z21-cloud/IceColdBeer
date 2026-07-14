using IceColdBeer.PlayerInput;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlatformMover _platformMover;

    private void Awake() 
    {
        _platformMover.Initialize(_inputManager);    
    }
}
