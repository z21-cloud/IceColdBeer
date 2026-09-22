using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfigs", menuName = "Level Generation/SpawnConfigs")]
public class SpawnConfigs : ScriptableObject
{
    [Header("Spawn Area")]
    [SerializeField] private float _borderOffset = 0.35f;
    [SerializeField] private int _difficultyLevel = 0;
    [SerializeField] private float _minYSpawnPosition = 0.75f;

    public float BorderOffset => _borderOffset;
    public int DifficultyLevel => _difficultyLevel;
    public float MinYSpawnPosition => _minYSpawnPosition;
}
