using IceColdBeer.Core;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreCounter
{
    private int _currentScore;
    public int CurrentScore => _currentScore;

    public void AddScore(int amount)
    {
        _currentScore += amount;
        Debug.Log($"[ScoreManager]: Score added. Current score is {_currentScore}");
    }
}
