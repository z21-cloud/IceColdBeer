using System;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class WinHole : MonoBehaviour
    {
        public event Action OnPlayerWon;

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.TryGetComponent<BallPickable>(out var ballPickable))
            {
                OnPlayerWon?.Invoke();
            }
        }
    }
}