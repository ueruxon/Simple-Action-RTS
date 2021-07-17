using UnityEngine;

[CreateAssetMenu(fileName = "Patrol", menuName = "Units State/Patrol")]
public class PatrolUnitState : UnitState_Base {
    private enum State {
        PatrollingMoving,
        PatrollingIdle,
    }

    private State _currentState;
    private Vector3 _startPosition;
    private Vector3 _patrolPosition;
    private float _patrolTimer;

    public override void Init() {
        _startPosition = BaseUnit.GetPosition();
        _currentState = State.PatrollingIdle;
    }

    public override void UpdateState() {
        switch (_currentState) {
            case State.PatrollingMoving:
                BaseUnit.SetDestination(_patrolPosition);

                float reachedDistance = 2f;
                if (Vector3.Distance(BaseUnit.GetPosition(), _patrolPosition) < reachedDistance) {
                    _currentState = State.PatrollingIdle;
                }
                break;
            case State.PatrollingIdle:
                _patrolTimer -= Time.deltaTime;
                if (_patrolTimer < 0f) {
                    _patrolTimer = Random.Range(0f, 3f);

                    _patrolPosition = _startPosition + GetRandomDirection() * Random.Range(0f, 10f);
                    _currentState = State.PatrollingMoving;
                }
                break;
        }
    }

    private Vector3 GetRandomDirection() {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}
