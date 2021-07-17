using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class FindTargetLogic : MonoBehaviour
{
    [SerializeField] private float _findTargetRadius = 10f;
    [SerializeField] private bool _targetIsEnemy;

    private Unit _unit;
    private float _findTargetTimer;

    private void Awake() {
        _unit = GetComponent<Unit>();
    }

    private void Update() {
        if (_unit.IsInAttack) {
            // Уже атакуем
            return;
        }

        _findTargetTimer -= Time.deltaTime;
        if (_findTargetTimer < 0f) {
            float findTargetTimerMax = .3f;
            _findTargetTimer += findTargetTimerMax;

            FindTarget();
        }
    }

    private void FindTarget() {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _findTargetRadius);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out Unit unit)) {
                if (unit.IsEnemy() != _targetIsEnemy) {
                    // Это враг, атакуем
                    _unit.SetTarget(unit);
                    return;
                }
            }

            if (collider.TryGetComponent(out Building building)) {
                if (building.IsEnemy() != _targetIsEnemy) {
                    // Это строение врага, атакуем
                    _unit.SetTarget(building);
                    return;
                }
            }
        }
    }
}
