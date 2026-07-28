using IceColdBeer.PlayerInput;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _minHeight = -5f;
        [SerializeField] private float _maxHeight = 5f;
        [SerializeField] private float _maxDiff = 2.5f;

        private float _leftHeight;
        private float _rightHeight;
        private float _platromLength;
        private IInputReader _inputReader;

        private void Awake()
        {
            _platromLength = transform.localScale.x;
            Debug.Log(_platromLength);
        }

        public void Initialize(IInputReader inputReader)
        {
            _inputReader = inputReader;    
        }


        private void Update()
        {
            float newLeft  = _leftHeight  + _inputReader.LeftCorner  * _speed * Time.deltaTime;
            float newRight = _rightHeight + _inputReader.RightCorner * _speed * Time.deltaTime;

            // 1. общий диапазон каждого края
            newLeft  = Mathf.Clamp(newLeft,  _minHeight, _maxHeight);
            newRight = Mathf.Clamp(newRight, _minHeight, _maxHeight);

            // 2. дополнительный лимит на РАЗНИЦУ между ними
            float clampedLeft  = Mathf.Clamp(newLeft,  _rightHeight - _maxDiff, _rightHeight + _maxDiff);
            float clampedRight = Mathf.Clamp(newRight, _leftHeight  - _maxDiff, _leftHeight  + _maxDiff);

            _leftHeight = clampedLeft;
            _rightHeight = clampedRight;

            float centerHeight = (_leftHeight + _rightHeight) / 2f;
            transform.position = new Vector3(transform.position.x, centerHeight, 0f);

            // tan between 2 floats
            float angle = Mathf.Atan2(_rightHeight - _leftHeight, _platromLength) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
