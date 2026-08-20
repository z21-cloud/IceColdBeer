using IceColdBeer.Core;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private IScoreCounter _scoreCounter;
    
    public void Intitialize(IScoreCounter scoreCounter)
    {
        Debug.Log("[UIManager]: UI Manager initialized.");
        _scoreCounter = scoreCounter;
        _scoreCounter.OnCoinPickedUp += UpdateScoreText;
        
        Debug.Log($"[UIManager]: Current score is {_scoreCounter.CurrentScore}");
        _scoreText.text = $"{_scoreCounter.CurrentScore}";
    }

    private void UpdateScoreText()
    {
        Debug.Log($"[UIManager]: Updating score text. Current score is {_scoreCounter.CurrentScore}");
        _scoreText.text = $"{_scoreCounter.CurrentScore}";
    }

    private void OnDisable()
    {
        if (_scoreCounter != null)
        {
            _scoreCounter.OnCoinPickedUp -= UpdateScoreText;
        }
    }
}
