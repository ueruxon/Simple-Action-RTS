using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static Unit CreateEnemy(Vector3 spawnPosition, Transform pfEnemyUnit) {
        Transform unitTransform = Instantiate(pfEnemyUnit, spawnPosition, Quaternion.identity);

        Unit enemyUnit = unitTransform.GetComponent<Unit>();
        return enemyUnit;
    }

    public event UnityAction<EnemySpawner> OnDestroyed; 

    [SerializeField] private Transform pfEnemyUnit;
    [SerializeField] private int _unitCountMax;
    [SerializeField] private float _spawnTimerMax;

    private List<Unit> _unitList;
    private float _spawnTimer;

    private void Awake() {
        _unitList = new List<Unit>();

        for (int i = 0; i < _unitCountMax; i++) {
            SpawnEnemy();
        }
    }

    private void Start() {
        GetComponent<Building>().OnDead += EnemySpawner_OnDead;

        GameManager.Instance.AddEnemySpawner(this);
    }

    private void EnemySpawner_OnDead() {
        OnDestroyed?.Invoke(this);
    }

    private void Update() {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0f) {
            _spawnTimer += _spawnTimerMax;
            if (_unitList.Count < _unitCountMax) {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy() {
        Vector3 spawnPosition = transform.position + UtilsClass.GetRandomDirXZ() * UnityEngine.Random.Range(2f, 7f);
        Unit enemyUnit = CreateEnemy(spawnPosition, pfEnemyUnit);

        _unitList.Add(enemyUnit);

        enemyUnit.OnDead += UnitOnDead;
    }

    private void UnitOnDead(Unit unit) {
        _unitList.Remove(unit);
    }
}
