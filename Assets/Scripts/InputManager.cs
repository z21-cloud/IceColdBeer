using System;
using UnityEngine;

namespace IceColdBeer.PlayerInput
{
    public class InputManager : MonoBehaviour, IInputReader
    {
        public float LeftCorner { get; private set; }
        public float RightCorner { get; private set; }
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            LeftCorner = Input.GetAxisRaw("Horizontal");
            RightCorner = Input.GetAxisRaw("Vertical");
        }
    }
}