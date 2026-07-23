using UnityEngine;

namespace IceColdBeer.Core
{
    public interface IPickable
    {
        public int Value { get; }
        public void PickUp();
    }
}
