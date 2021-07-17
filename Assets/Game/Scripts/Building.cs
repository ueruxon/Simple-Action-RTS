using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IUnitDamageable
{
    [SerializeField] private int _healthAmountMax;
    [SerializeField] private bool _isEnemy;
    [SerializeField] private float _attackDistanceOffset;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = new HealthSystem(_healthAmountMax);
    }

    public bool IsEnemy() {
        return _isEnemy;
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    public Transform GetTransform() {
        return transform;
    }

    public float GetAttackDistanceOffset() {
        return _attackDistanceOffset;
    }
}
