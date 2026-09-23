using System;
using System.Collections.Generic;
using IceColdBeer.Core;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Level
{
    public class LevelGenerator : MonoBehaviour //, ICoinCounter
    {
        [Header("Spawn Area")]
        [SerializeField] private SpriteRenderer _spawnArea;

        [Header("Generation Rules")]
        [SerializeField] private GenerationRules _generationRules;

        [Header("Player Spawn Position")]
        [SerializeField] private Transform _playerSpawnPosition;

        [Header("Object Types")]
        [SerializeField] private ObjectType[] _objectTypes;

        [Header("Debug")]
        [SerializeField] private bool _debugMode;

        // pools
        private CoinPool _coinPool;
        private HolePool _loseHolePool;
        private WinHolePool _winHolePool;

        //
        private IScoreCounter _scoreCounter;

        //
        private BFS _bfs;
        private GridBuilder _gridBuilder;
        private IsValidPosition _isValidPosition;

        // area bounds and spawned positions
        private Bounds _spawnAreaBounds;
        private Vector2 _winHolePosition;
        private List<Vector2> _spawnedPositionsLoseHole;
        private List<Vector2> _spawnedPositionsCoins;
        private List<Goal> _goals;

        // private variables
        private float _currentMinYSpawnPosition = 0f;

        // public properties
        // public int CoinsCount => _generationRules.CoinsCount;

        public void Initailize(
            HolePool loseHolePool,
            CoinPool coinPool,
            WinHolePool winHolePool,
            IScoreCounter scoreCounter)
        {
            _loseHolePool = loseHolePool;
            _coinPool = coinPool;
            _winHolePool = winHolePool;
            _scoreCounter = scoreCounter;

            _bfs = new BFS();
            _gridBuilder = new GridBuilder(_generationRules.GridBuilderConfig.NodeSize, _generationRules.GridBuilderConfig.UnwalkableLayerMask);
            _isValidPosition = new IsValidPosition();
        }

        private void Awake()
        {
            if (_generationRules.SpawnConfigs == null || _generationRules.GridBuilderConfig == null)
            {
                Debug.LogError($"[LevelGenerator] Generation Rules is not assigned!");
                return;
            }

            if (_spawnArea == null)
            {
                Debug.LogError($"[LevelGenerator] Spawn Area is not assigned!");
                return;
            }

            if (_loseHolePool == null)
            {
                Debug.LogError($"[LevelGenerator] Hole Pool is not assigned!");
                return;
            }

            if (_coinPool == null)
            {
                Debug.LogError($"[LevelGenerator] Coin Pool is not assigned!");
                return;
            }

            if (_winHolePool == null)
            {
                Debug.LogError($"[LevelGenerator] Win Hole Pool is not assigned!");
                return;
            }

            _spawnAreaBounds = _spawnArea.bounds;

            _spawnedPositionsCoins = new();
            _spawnedPositionsLoseHole = new();
            _goals = new();

            // Initialize random seed for deterministic level generation before level generation
            UnityEngine.Random.InitState(_generationRules.Seed);

            GenerateLevel();
            Physics2D.SyncTransforms();
            _gridBuilder.BuildGrid(_spawnAreaBounds);

            List<Goal> invalidGoals = FindInvalidGoals();

            if (invalidGoals.Count > 0)
            {
                Debug.LogWarning($"[Level Generator] Needs to regenerate for {invalidGoals.Count} objects");
                RegenerateInvalidGoals(invalidGoals);
                Physics2D.SyncTransforms();
            }
        }

        private List<Goal> FindInvalidGoals()
        {
            List<Goal> invalids = new();
            foreach (var goal in _goals)
            {
                if (IsPathAvailable(_playerSpawnPosition.position, goal.Position)) // goalPosition.Key is the position of the goal (win hole or coin)
                {
                    if (_debugMode)
                    {
                        Debug.Log($"[LevelGenerator] Path found between player and goal position: {goal.Type} : {goal.Position}");
                    }
                }
                else
                {
                    if (_debugMode)
                    {
                        Debug.LogError($"[LevelGenerator] No path found between player and goal position: {goal.Type} : {goal.Position}");
                    }

                    // if IsPathAvailable can't find path, add position & object type to _goalsInvalidPositions to regenerate invalid positions
                    invalids.Add(goal);
                }
            }

            return invalids;
        }

        private void RegenerateInvalidGoals(List<Goal> invalidGoals)
        {
            List<Goal> temp = new(invalidGoals);
            int index = 0;
            foreach (Goal tempGoal in temp)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Vector2 rndPosition = ObjectRandomPosition(tempGoal.Type);
                    if (IsPathAvailable(_playerSpawnPosition.position, rndPosition))
                    {
                        Debug.LogWarning($"[Level Generator] Regenerated new position succesfully {rndPosition}");
                        var regeneratedGoal = invalidGoals[index];
                        regeneratedGoal.Position = rndPosition;
                        break;
                    }
                }

                index++;
            }
        }

        private bool IsPathAvailable(Vector3 playerPosition, Vector2 goalPosition)
        {
            Node startNode;
            if (_gridBuilder.TryGetNodePosition(playerPosition, out startNode)) { }

            Node targetNode;
            if (_gridBuilder.TryGetNodePosition(goalPosition, out targetNode)) { }

            if (_debugMode)
            {
                Debug.Log($"[LG] start: {startNode.gridPosition} walkable={startNode.isWalkable} " + $"nodePos={startNode.position} playerPos={playerPosition}");
                Debug.Log($"[LG] target: {targetNode.gridPosition} walkable={targetNode.isWalkable} " + $"nodePos={targetNode.position} goalPos={goalPosition}");
            }

            var nodes = _gridBuilder.Grid;

            if (_bfs.FindPath(startNode, targetNode, nodes))
            {
                if (_debugMode)
                {
                    Debug.Log($"[LevelGenerator] Path found between start and target nodes!");
                }
                return true;
            }
            else
            {
                if (_debugMode)
                {
                    Debug.LogError($"[LevelGenerator] No path found between start and target nodes!");
                }
                return false;
            }
        }

        private void GenerateLevel()
        {
            GenerateWinHole();
            GenerateCoins();
            GenerateLoseHoles();
        }

        #region GENERATOR:WIN HOLE POSITION
        private void GenerateWinHole()
        {
            var winHole = _winHolePool.GetHole();
            if (winHole != null)
            {
                _scoreCounter.Subscribe(winHole, _generationRules.CoinsCount);
                float yDifficultyOffset = ApplyDifficultyOffset(_generationRules.SpawnConfigs.DifficultyLevel);
                winHole.transform.position = ObjectRandomPosition(ObjectType.WinHole, maxAttempts: 10000, yDifficultyOffset: yDifficultyOffset);
                _winHolePosition = winHole.transform.position;
                Goal newGoal = new(ObjectType.WinHole, _winHolePosition);
                _goals.Add(newGoal);
            }
            else
            {
                if (_debugMode)
                {
                    Debug.LogWarning($"[LevelGenerator] Win Hole Pool is empty, cannot generate win hole!");
                }
            }
        }

        private float ApplyDifficultyOffset(int difficultyLevel)
        {
            _currentMinYSpawnPosition = _generationRules.SpawnConfigs.MinYSpawnPosition + (difficultyLevel * 0.5f);
            _currentMinYSpawnPosition = Mathf.Clamp(_currentMinYSpawnPosition, _generationRules.SpawnConfigs.MinYSpawnPosition, _spawnAreaBounds.max.y - _generationRules.SpawnConfigs.BorderOffset);
            return _currentMinYSpawnPosition;
        }
        #endregion

        #region GENERATOR:COIN POSITION
        private void GenerateCoins()
        {
            for (int i = 0; i < _generationRules.CoinsCount; i++)
            {
                var coin = _coinPool.GetCoin();
                if (coin != null)
                {
                    coin.transform.position = ObjectRandomPosition(ObjectType.Coin);
                    Vector2 coinPosition = coin.transform.position;
                    _spawnedPositionsCoins.Add(coinPosition);
                    Goal newGoal = new(ObjectType.Coin, coinPosition);
                    _goals.Add(newGoal);
                }
            }
        }
        #endregion

        #region  GENERATOR:LOSE HOLE POSIION
        private void GenerateLoseHoles()
        {
            for (int i = 0; i < _generationRules.LoseHoleCount; i++)
            {
                var hole = _loseHolePool.GetHole();
                if (hole != null)
                {
                    hole.transform.position = ObjectRandomPosition(ObjectType.LoseHole);
                    _spawnedPositionsLoseHole.Add(hole.transform.position);
                }
            }
        }

        #endregion

        #region GENERATOR:HELPER METHODS
        private Vector2 ObjectRandomPosition(ObjectType objectType, int maxAttempts = 10000, float yDifficultyOffset = 0f)
        {
            float minYPosition = yDifficultyOffset == 0f ? _generationRules.SpawnConfigs.MinYSpawnPosition : yDifficultyOffset;
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 randomPosition = GenerateRandomPosition(minYPosition);
                if (_isValidPosition.ObjectPosition(randomPosition, _playerSpawnPosition.position, _generationRules, objectType, _winHolePosition, _spawnedPositionsCoins, _spawnedPositionsLoseHole))
                {
                    return randomPosition;
                }
            }

            if (_debugMode)
            {
                Debug.LogWarning($"[LevelGenerator] Could not find a valid position for {objectType} after {maxAttempts} attempts.");
            }
            return Vector2.zero;
        }

        private Vector2 GenerateRandomPosition(float minYPosition = 0f)
        {
            float randomX = UnityEngine.Random.Range(
                _spawnAreaBounds.min.x + _generationRules.SpawnConfigs.BorderOffset,
                _spawnAreaBounds.max.x - _generationRules.SpawnConfigs.BorderOffset
                );

            float randomY = UnityEngine.Random.Range(
                minYPosition,
                _spawnAreaBounds.max.y - _generationRules.SpawnConfigs.BorderOffset
                );

            return new Vector2(randomX, randomY);
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (_gridBuilder == null || !_debugMode) return;

            for (int i = 0; i < _gridBuilder.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < _gridBuilder.Grid.GetLength(1); j++)
                {
                    Node node = _gridBuilder.Grid[i, j];
                    Gizmos.color = node.isWalkable ? Color.green : Color.red;
                    Gizmos.DrawWireSphere(node.position, _generationRules.GridBuilderConfig.NodeSize * 0.35f);
                }
            }
        }
    }
}