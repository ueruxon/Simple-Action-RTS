using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event UnityAction EnemySpawnedDestroyed;

    private List<EnemySpawner> _enemySpawnerList;

    private void Awake() {
        Instance = this;

        _enemySpawnerList = new List<EnemySpawner>();
    }

    public void AddEnemySpawner(EnemySpawner enemySpawner) {
        _enemySpawnerList.Add(enemySpawner);
        enemySpawner.OnDestroyed += EnemySpawner_OnDestroyed;
    }

    private void EnemySpawner_OnDestroyed(EnemySpawner enemySpawner) {
        _enemySpawnerList.Remove(enemySpawner);

        EnemySpawnedDestroyed?.Invoke();

        if (_enemySpawnerList.Count == 0) {
            // Game Win
            GameWin();
        }
    }

    public int GetEnemySpawnerAliveCount() {
        return _enemySpawnerList.Count;
    }

    private void GameWin() {
        Debug.Log("Game Win");
    }
}
