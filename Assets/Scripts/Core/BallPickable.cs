using IceColdBeer.Core;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class BallPickable : MonoBehaviour
    {
        private IScoreCounter _scoreCounter;

        public void Initailize(IScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<IPickable>(out var pickable))
            {
                pickable.PickUp();

                _scoreCounter?.AddScore(pickable.Value);
            }
        }
    }
}
