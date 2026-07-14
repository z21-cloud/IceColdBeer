using UnityEngine;

namespace IceColdBeer.PlayerInput
{
    public interface IInputReader
    {
        public float LeftCorner { get; }
        public float RightCorner { get; }
    }
}
