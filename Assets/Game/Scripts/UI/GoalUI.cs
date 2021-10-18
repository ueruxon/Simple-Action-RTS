using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _enemySpawnerCount;

    private void Start() {
        GameManager.Instance.EnemySpawnedDestroyed += OnEnemySpawnedDestroyed;
        UpdateText();
    }

    private void OnEnemySpawnedDestroyed() {
        UpdateText();
    }

    private void UpdateText() {
        _enemySpawnerCount.SetText(GameManager.Instance.GetEnemySpawnerAliveCount().ToString());
    }
}
