using System;
using System.Collections.Generic;
using IceColdBeer.Core;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Level
{
    public class LevelGenerator : MonoBehaviour, ICoinCounter
    {
        //[NOTE]: numbers to data driven scriptable object for level generation
        [Header("Spawn Area")]
        [SerializeField] private SpriteRenderer _spawnArea;
        [SerializeField] private float _borderOffset = 0.35f;
        [SerializeField] private int _difficultyLevel = 0;
        
        [Header("Lose Holes")]
        [SerializeField] private int _loseHoleCount = 10;
        [SerializeField] private float _minDistanceBetweenLoseHoles = .25f;
        
        [Header("Player Spawn Position")]
        [SerializeField] private Transform _playerSpawnPosition;
        [SerializeField] private float _minDistanceBetweenPlayer = .25f;
       
        [Header("Win Hole")]
        [SerializeField] private float _minDistanceBetweenWinHole = .25f;
       
        [Header("Coins")]
        [SerializeField] private int _coinsCount = 2;
        [SerializeField] private float _minDistanceBetweenCoins = 1f;
        
        // pools
        private CoinPool _coinPool;
        private HolePool _loseHolePool;
        private WinHolePool _winHolePool;

        //
        private IScoreCounter _scoreCounter;

        // area bounds and spawned positions
        private Bounds _spawnAreaBounds;
        private List<Vector2> _spawnedPositionsLoseHole;
        private List<Vector2> _spawnedPositionsCoins;
        private Vector2 _winHolePosition;

        // private variables
        private float _currentMinYSpawnPosition = 0f;

        // consts
        private const float _minYSpawnPosition = 0.75f;

        public int CoinsCount => _coinsCount;

        public void Initailize(HolePool loseHolePool, 
                            CoinPool coinPool, 
                            WinHolePool winHolePool, 
                            IScoreCounter scoreCounter)
        {
            _loseHolePool = loseHolePool;
            _coinPool = coinPool;
            _winHolePool = winHolePool;
            _scoreCounter = scoreCounter;
        }

        private void Awake()
        {
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

            if(_coinPool == null)
            {
                Debug.LogError($"[LevelGenerator] Coin Pool is not assigned!");
                return;
            }

            _spawnAreaBounds = _spawnArea.bounds;

            _spawnedPositionsCoins = new();
            _spawnedPositionsLoseHole = new();

            GenerateLevel();
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
            if(winHole != null)
            {
                _scoreCounter.Subscribe(winHole, _coinsCount);
                winHole.transform.position = GetRandomPositionWinHole();
                _winHolePosition = winHole.transform.position;
            }
            else
            {
                Debug.LogWarning($"[LevelGenerator] Win Hole Pool is empty, cannot generate win hole!");
            }
        }

        private Vector2 GetRandomPositionWinHole(int maxAttempts = 1000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                float yDifficultyOffset = ApplyDifficultyOffset(_difficultyLevel);
                Vector2 randomPosition = GenerateRandomPosition(yDifficultyOffset);
                if (IsValidWinHolePosition(randomPosition))
                {
                    return randomPosition;
                }
            }

            Debug.LogWarning($"[LevelGenerator] Could not find a valid position for win hole after {maxAttempts} attempts.");

            return Vector2.zero;
        }

        private float ApplyDifficultyOffset(int difficultyLevel)
        {
            _currentMinYSpawnPosition = _minYSpawnPosition + (difficultyLevel * 0.5f);
            _currentMinYSpawnPosition = Mathf.Clamp(_currentMinYSpawnPosition, _minYSpawnPosition, _spawnAreaBounds.max.y - _borderOffset);
            return _currentMinYSpawnPosition;
        }

        // Needs to check distance between player & win hole, because win hole generates first
        private bool IsValidWinHolePosition(Vector2 position)
        {
            if (Vector2.Distance(position, _playerSpawnPosition.position) < _minDistanceBetweenPlayer)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region GENERATOR:COIN POSITION
        private void GenerateCoins()
        {
            for(int i = 0; i < _coinsCount; i++)
            {
                var coin = _coinPool.GetCoin();
                if(coin != null)
                {
                    coin.transform.position = GetRandomPositionCoin();
                    _spawnedPositionsCoins.Add(coin.transform.position);
                }
            }
        }

        private Vector2 GetRandomPositionCoin(int maxAttempts = 1000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 randomPosition = GenerateRandomPosition(0);
                if (IsValidPositionCoin(randomPosition))
                {
                    return randomPosition;
                }
            }

            Debug.LogWarning($"[LevelGenerator] Could not find a valid position for coin after {maxAttempts} attempts.");

            return Vector2.zero;
        }

        // Need to check distance between player & win hole and other coins
        private bool IsValidPositionCoin(Vector2 position)
        {
            if (Vector2.Distance(position, _playerSpawnPosition.position) < _minDistanceBetweenPlayer)
            {
                return false;
            }

            if(Vector2.Distance(position, _winHolePosition) < _minDistanceBetweenWinHole)
            {
                return false;
            }

            foreach (var spawnedCoinPosition in _spawnedPositionsCoins)
            {
                if (Vector2.Distance(position, spawnedCoinPosition) < _minDistanceBetweenCoins)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region  GENERATOR:LOSE HOLE POSIION
        private void GenerateLoseHoles()
        {
            for (int i = 0; i < _loseHoleCount; i++)
            {
                var hole = _loseHolePool.GetHole();
                if (hole != null)
                {
                    hole.transform.position = GetRandomPositionLoseHole();
                    _spawnedPositionsLoseHole.Add(hole.transform.position);
                }
            }
        }

        private Vector2 GetRandomPositionLoseHole(int maxAttempts = 1000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 randomPosition = GenerateRandomPosition(0);
                if (IsValidPositionLoseHole(randomPosition))
                {
                    return randomPosition;
                }
            }

            Debug.LogWarning($"[LevelGenerator] Could not find a valid position for lose hole after {maxAttempts} attempts.");

            return Vector2.zero;
        }

        // Need to check distance between player, win hole, coins & other lose holes
        private bool IsValidPositionLoseHole(Vector2 position)
        {
            if (Vector2.Distance(position, _playerSpawnPosition.position) < _minDistanceBetweenPlayer)
            {
                return false;
            }

            if(Vector2.Distance(position, _winHolePosition) < _minDistanceBetweenWinHole)
            {
                return false;
            }

            foreach (var spawnedCoinPosition in _spawnedPositionsCoins)
            {
                if (Vector2.Distance(position, spawnedCoinPosition) < _minDistanceBetweenCoins)
                {
                    return false;
                }
            }

            foreach (var spawnedPosition in _spawnedPositionsLoseHole)
            {
                if (Vector2.Distance(position, spawnedPosition) < _minDistanceBetweenLoseHoles)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        // gets random position inside bounderies + border offset
        private Vector2 GenerateRandomPosition(float minYPosition = _minYSpawnPosition)
        {
            float randomX = UnityEngine.Random.Range(
                _spawnAreaBounds.min.x + _borderOffset,
                _spawnAreaBounds.max.x - _borderOffset
                );

            float randomY = UnityEngine.Random.Range(
                minYPosition,
                _spawnAreaBounds.max.y - _borderOffset
                );
        
            return new Vector2(randomX, randomY);
        }
    }
}
