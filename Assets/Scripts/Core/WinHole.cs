using IceColdBeer.Core;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class WinHole : MonoBehaviour
    {
        private IScoreCounter _scoreCounter;
        private ICoinCounter _coinCounter;
        public void Initialize(IScoreCounter scoreCounter, ICoinCounter coinCounter)
        {
            _scoreCounter = scoreCounter;
            _coinCounter = coinCounter;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(_scoreCounter.CurrentScore == _coinCounter.CoinsCount)
            {
                Debug.Log($"Player wins!");
            }
        }
    }
}