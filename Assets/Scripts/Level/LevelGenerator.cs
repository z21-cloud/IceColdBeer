using System.Collections.Generic;
using IceColdBeer.Pools;
using UnityEngine;

namespace IceColdBeer.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spawnArea;
        [SerializeField] private int _numberOfHoles = 10;
        [SerializeField] private float _minDistanceBetweenHoles = .25f;
        [SerializeField] private float _minDistanceBetweenPlayer = .25f;
        [SerializeField] private float _minDistanceBetweenCoins = 1f;
        [SerializeField] private HolePool _holePool;
        [SerializeField] private CoinPool _coinPool;
        [SerializeField] private Transform _playerSpawnPosition;

        private Bounds _spawnAreaBounds;
        private Vector2 _spawnAreaMin;
        private Vector2 _spawnAreaMax;
        private List<Vector2> _spawnedPositions;
        private List<Transform> _spawnedCoins;

        private void Awake()
        {
            if (_spawnArea == null)
            {
                Debug.LogError($"[LevelGenerator] Spawn Area is not assigned!");
                return;
            }

            if (_holePool == null)
            {
                Debug.LogError($"[LevelGenerator] Hole Pool is not assigned!");
                return;
            }

            _spawnedPositions = new();
            _spawnedCoins = new();
            _spawnedCoins = _coinPool.GetSpawnPoints();

            GenerateLevel();
        }

        private void GenerateLevel()
        {
            _spawnAreaBounds = _spawnArea.bounds;
            _spawnAreaMin = _spawnAreaBounds.min;
            _spawnAreaMax = _spawnAreaBounds.max;

            for (int i = 0; i < _numberOfHoles; i++)
            {
                var hole = _holePool.GetHole();
                if (hole != null)
                {
                    hole.transform.position = GetRandomPositionInSpawnArea();
                }
            }
        }

        private Vector2 GetRandomPositionInSpawnArea()
        {
            float randomX = UnityEngine.Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
            float randomY = UnityEngine.Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
            Vector2 vector2 = new Vector2(randomX, randomY);

            if (Vector2.Distance(_playerSpawnPosition.position, vector2) < _minDistanceBetweenPlayer)
            {
                return GetRandomPositionInSpawnArea();
            }

            foreach (var pos in _spawnedCoins)
            {
                if (Vector2.Distance(pos.position, vector2) < _minDistanceBetweenCoins)
                {
                    return GetRandomPositionInSpawnArea();
                }
            }

            foreach (var pos in _spawnedPositions)
            {
                if (Vector2.Distance(pos, vector2) < _minDistanceBetweenHoles)
                {
                    return GetRandomPositionInSpawnArea();
                }
            }

            _spawnedPositions.Add(vector2);
            return vector2;
        }
    }
}
