using IceColdBeer.PlayerInput;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private float _leftHeight;
        private float _rightHeight;
        private IInputReader _inputReader;

        public void Initialize(IInputReader inputReader)
        {
            _inputReader = inputReader;    
        }

        private void Update()
        {
            _leftHeight += _inputReader.LeftCorner * _speed * Time.deltaTime;
            _rightHeight += _inputReader.RightCorner * _speed * Time.deltaTime;
        }
    }
}
