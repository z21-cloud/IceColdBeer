using System;
using IceColdBeer.Core;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreCounter
{
    private int _coinsCount;
    private int _currentScore;
    public int CurrentScore => _currentScore;

    public event Action OnCoinPickedUp;
    private WinHole _winHole;

    public void AddScore(int amount)
    {
        _currentScore += amount;
        Debug.Log($"[ScoreManager]: Score added. Current score is {_currentScore}");
        OnCoinPickedUp?.Invoke();
    }

    public void Subscribe(WinHole winHole, int coinsCount)
    {
        _coinsCount = coinsCount;
        _winHole = winHole;
        _winHole.OnPlayerWon += PlayerWon;
    }

    private void PlayerWon()
    {
        if(_currentScore == _coinsCount)
        {
            Debug.Log($"Player Wins!");
        }
        else
        {
            Debug.Log($"Not enough coins!");
        }
    }

    private void OnDestroy()
    {
        _winHole.OnPlayerWon -= PlayerWon;
    }
}
