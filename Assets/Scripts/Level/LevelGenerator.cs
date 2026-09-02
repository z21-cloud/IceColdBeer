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

        // area bounds and spawned positions
        private Bounds _spawnAreaBounds;
        private List<Vector2> _spawnedPositionsLoseHole;
        private List<Vector2> _spawnedPositionsCoins;
        private Vector2 _winHolePosition;

        public int CoinsCount => _coinsCount;

        public void Initailize(HolePool loseHolePool, 
                            CoinPool coinPool, 
                            WinHolePool winHolePool, 
                            IScoreCounter scoreCounter)
        {
            _loseHolePool = loseHolePool;
            _coinPool = coinPool;
            _winHolePool = winHolePool;

            _winHolePool.Initialize(scoreCounter, this);
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

        private void GenerateWinHole()
        {
            var winHole = _winHolePool.GetHole();
            if(winHole != null)
            {
                winHole.transform.position = GetRandomPositionInSpawnArea();
                _winHolePosition = winHole.transform.position;
            }
            else
            {
                Debug.LogWarning($"[LevelGenerator] Win Hole Pool is empty, cannot generate win hole!");
            }
        }

        private void GenerateCoins()
        {
            for(int i = 0; i < _coinsCount; i++)
            {
                var coin = _coinPool.GetCoin();
                if(coin != null)
                {
                    coin.transform.position = GetRandomPositionInSpawnArea();
                    _spawnedPositionsCoins.Add(coin.transform.position);
                }
            }
        }

        private void GenerateLoseHoles()
        {
            for (int i = 0; i < _loseHoleCount; i++)
            {
                var hole = _loseHolePool.GetHole();
                if (hole != null)
                {
                    hole.transform.position = GetRandomPositionInSpawnArea();
                    _spawnedPositionsLoseHole.Add(hole.transform.position);
                }
            }
        }

        private Vector2 GetRandomPositionInSpawnArea(int maxAttempts = 1000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 randomPosition = GenerateRandomPosition();
                if (IsValidPosition(randomPosition))
                {
                    return randomPosition;
                }
            }

            Debug.LogWarning($"[LevelGenerator] Could not find a valid position after {maxAttempts} attempts.");

            return Vector2.zero;
        }

        private bool IsValidPosition(Vector2 position)
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

        private Vector2 GenerateRandomPosition()
        {
            float randomX = UnityEngine.Random.Range(
                _spawnAreaBounds.min.x + _borderOffset,
                _spawnAreaBounds.max.x - _borderOffset
                );

            float randomY = UnityEngine.Random.Range(
                _spawnAreaBounds.min.y + _borderOffset,
                _spawnAreaBounds.max.y - _borderOffset
                );
            
            return new Vector2(randomX, randomY);
        }
    }
}
