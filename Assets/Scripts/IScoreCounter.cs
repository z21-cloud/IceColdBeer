using System;
using UnityEngine;

namespace IceColdBeer.Core
{
    public interface IScoreCounter
    {
        public int CurrentScore { get; }
        public void AddScore(int amount);
        public event Action OnCoinPickedUp;
        public void Subscribe(WinHole winHole, int coinsCount);
    }
}
