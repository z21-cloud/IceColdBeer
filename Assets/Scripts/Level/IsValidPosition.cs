using UnityEngine;
using System.Collections.Generic;

public class IsValidPosition
{
    public bool ObjectPosition(Vector2 objectRandomPosition, Vector2 playerSpawnPosition, GenerationRules generationRules, ObjectType objectType,
                           Vector2 winHolePosition, List<Vector2> spawnedPositionsCoins, List<Vector2> spawnedPositionsLoseHoles)
    {
        if (objectType == ObjectType.WinHole)
        {
            return WinHolePosition(
                objectRandomPosition,
                playerSpawnPosition,
                generationRules.MinDistanceBetweenPlayer,
                spawnedPositionsLoseHoles,
                generationRules.MinDistanceBetweenLoseHoles);
        }

        if (objectType == ObjectType.Coin)
        {
            return CoinPosition(
                objectRandomPosition,
                playerSpawnPosition,
                winHolePosition,
                spawnedPositionsCoins,
                generationRules.MinDistanceBetweenPlayer,
                generationRules.MinDistanceBetweenWinHole,
                generationRules.MinDistanceBetweenCoins,
                spawnedPositionsLoseHoles,
                generationRules.MinDistanceBetweenLoseHoles);
        }

        else if (objectType == ObjectType.LoseHole)
        {
            return LoseHolePosition(
                objectRandomPosition,
                playerSpawnPosition,
                winHolePosition,
                spawnedPositionsCoins,
                spawnedPositionsLoseHoles,

                generationRules.MinDistanceBetweenPlayer,
                generationRules.MinDistanceBetweenWinHole,
                generationRules.MinDistanceBetweenCoins,
                generationRules.MinDistanceBetweenLoseHoles);
        }
        
        return false;
    }

    private bool WinHolePosition(Vector2 spawnPosition, Vector2 playerSpawnPosition, float minDistanceBetweenPlayer, List<Vector2> spawnedPositionsLoseHoles = null, float minDistanceBetweenLoseHoles = 0f)
    {
        if (Vector2.Distance(spawnPosition, playerSpawnPosition) < minDistanceBetweenPlayer)
        {
            return false;
        }

        foreach (var pos in spawnedPositionsLoseHoles)
        {
            if (Vector2.Distance(spawnPosition, pos) < minDistanceBetweenLoseHoles) return false;
        }

        return true;
    }

    private bool CoinPosition(
        Vector2 spawnPosition,
        Vector2 playerSpawnPosition,
        Vector2 winHolePosition,
        List<Vector2> spawnedPositionsCoins,
        float minDistanceBetweenPlayer,
        float minDistanceBetweenWinHole,
        float minDistanceBetweenCoins,
        List<Vector2> spawnedPositionsLoseHoles = null,
        float minDistanceBetweenLoseHoles = 0f)
    {
        if (Vector2.Distance(spawnPosition, playerSpawnPosition) < minDistanceBetweenPlayer)
        {
            return false;
        }

        if (Vector2.Distance(spawnPosition, winHolePosition) < minDistanceBetweenWinHole)
        {
            return false;
        }

        foreach (var spawnedCoinPosition in spawnedPositionsCoins)
        {
            if (Vector2.Distance(spawnPosition, spawnedCoinPosition) < minDistanceBetweenCoins)
            {
                return false;
            }
        }

        foreach (var pos in spawnedPositionsLoseHoles)
        {
            if (Vector2.Distance(spawnPosition, pos) < minDistanceBetweenLoseHoles) return false;
        }

        return true;
    }

    private bool LoseHolePosition(
        Vector2 spawnPosition,
        Vector2 playerSpawnPosition,
        Vector2 winHolePosition,
        List<Vector2> spawnedPositionsCoins,
        List<Vector2> spawnedPositionsLoseHole,
        float minDistanceBetweenPlayer,
        float minDistanceBetweenWinHole,
        float minDistanceBetweenCoins,
        float minDistanceBetweenLoseHoles)

    {
        if (Vector2.Distance(spawnPosition, playerSpawnPosition) < minDistanceBetweenPlayer)
        {
            return false;
        }

        if (Vector2.Distance(spawnPosition, winHolePosition) < minDistanceBetweenWinHole)
        {
            return false;
        }

        foreach (var spawnedCoinPosition in spawnedPositionsCoins)
        {
            if (Vector2.Distance(spawnPosition, spawnedCoinPosition) < minDistanceBetweenCoins)
            {
                return false;
            }
        }

        foreach (var spawnedPosition in spawnedPositionsLoseHole)
        {
            if (Vector2.Distance(spawnPosition, spawnedPosition) < minDistanceBetweenLoseHoles)
            {
                return false;
            }
        }

        return true;
    }
}
