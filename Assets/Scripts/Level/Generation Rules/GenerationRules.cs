using UnityEngine;

[CreateAssetMenu(fileName = "GenerationRules", menuName = "Level Generation/GenerationRules")]
public class GenerationRules : ScriptableObject
{
    [Header("Level Generation Rules: Counts")]
    [SerializeField] private int _coinsCount = 2;
    [SerializeField] private int _loseHoleCount = 10;

    [Header("Level Generation Rules: Minimum Distances")]
    [SerializeField] private float _minDistanceBetweenCoins = 1.0f;
    [SerializeField] private float _minDistanceBetweenWinHole = .25f;
    [SerializeField] private float _minDistanceBetweenPlayer = .25f;
    [SerializeField] private float _minDistanceBetweenLoseHoles = .25f;

    [Header("Level Generation Rules: Seed")]
    [SerializeField] private int _seed = 0;

    [Header("Grid Builder Config")]
    [SerializeField] private GridBuilderConfig _gridBuilderConfig;

    [Header("Spawn Area")]
    [SerializeField] private SpawnConfigs _spawnConfigs;

    public float MinDistanceBetweenCoins => _minDistanceBetweenCoins;
    public float MinDistanceBetweenPlayer => _minDistanceBetweenPlayer;
    public float MinDistanceBetweenWinHole => _minDistanceBetweenWinHole;
    public float MinDistanceBetweenLoseHoles => _minDistanceBetweenLoseHoles;

    public int CoinsCount => _coinsCount;
    public int LoseHoleCount => _loseHoleCount;
    
    public int Seed => _seed;

    public GridBuilderConfig GridBuilderConfig => _gridBuilderConfig;
    public SpawnConfigs SpawnConfigs => _spawnConfigs;
}
