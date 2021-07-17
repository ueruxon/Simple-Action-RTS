using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacking", menuName = "Units State/Attacking")]
public class AttackUnitState : UnitState_Base {
    private enum State {
        MoveToTarget,
        AttackingTarget,
    }

    [SerializeField] protected int _damageAmount;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _attackTimerMax;

    private State _currentState;
    private float _attackTimer;
    protected IUnitDamageable _targetUnit;
    private UnitVisual _unitVisual;

    public override void Init() {
        _unitVisual = BaseUnit.GetUnitVisualObject();
    }

    public virtual void SetEnemyTarget(IUnitDamageable targetUnit) {
        _targetUnit = targetUnit;
        _currentState = State.MoveToTarget;
    }

    public override void UpdateState() {
        if (_targetUnit == null || _targetUnit.IsDead()) {
            StateIsFinished = true;
            BaseUnit.SetStateByDefualt();
            return;
        }

        switch (_currentState) {
            case State.MoveToTarget:
                // двигаемся к цели
                BaseUnit.SetDestination(_targetUnit.GetPosition());

                // дистанция позволяет атаковать
                if (Vector3.Distance(BaseUnit.GetPosition(), _targetUnit.GetPosition()) < GetAttackRange() + _targetUnit.GetAttackDistanceOffset()) {
                    // атака
                    BaseUnit.StopMoving();
                    _currentState = State.AttackingTarget;
                }
                break;

            case State.AttackingTarget:
                if (AttackingTarget()) {
                    if (_targetUnit.IsDead()) {
                        StateIsFinished = true;
                        BaseUnit.SetStateByDefualt();
                    }
                    else {
                        _currentState = State.MoveToTarget;
                    }
                }
                break;
        }
    }

    protected virtual bool AttackingTarget() {
        // Смотрим в направление врага
        BaseUnit.transform.LookAt(_targetUnit.GetPosition());

        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0) {
            _attackTimer += _attackTimerMax;

            // анимация
            _unitVisual.Attack();

            _targetUnit.Damage(_damageAmount);

            return true;
        }

        return false;
    }

    protected virtual float GetAttackRange() {
        return _attackRange;
    }
}
