using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayRandom : MonoBehaviour
{
    [SerializeField] private GenerationRules _generationRules;
    public void LoadPlaySceneRandom()
    {
        _generationRules.SetSeed();
        SceneManager.LoadScene(1);
    }
}
