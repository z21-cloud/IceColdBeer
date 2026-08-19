using IceColdBeer.Core;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int _currentScore;
    public void Intitialize(IScoreCounter scoreCounter)
    {
        Debug.Log("[UIManager]: UI Manager initialized.");
        _currentScore = scoreCounter.CurrentScore;
        Debug.Log($"[UIManager]: Current score is {_currentScore}");
    }
}
