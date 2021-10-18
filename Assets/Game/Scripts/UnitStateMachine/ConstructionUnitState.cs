using UnityEngine;

[CreateAssetMenu(fileName = "Construction", menuName = "Units State/Construction")]
public class ConstructionUnitState : UnitState_Base {
    private enum State {
        GoingToBuilding,
        Constructing,
    }

    private State _currentState;
    private BuildingConstruction _buildingConstruction;
    private Building _building;
    private UnitVisual _unitVisualObject;
    private float _constructionTimer;

    public override void Init() {
        _unitVisualObject = BaseUnit.GetUnitVisualObject();
    }

    public void SetBuildingConstruction(BuildingConstruction buildingConstruction) {
        _buildingConstruction = buildingConstruction;
        _building = buildingConstruction.gameObject.GetComponent<Building>();
        _currentState = State.GoingToBuilding;
    }

    public override void UpdateState() {
        switch (_currentState) {
            case State.GoingToBuilding:
                BaseUnit.SetDestination(_building.GetPosition());

                if (Vector3.Distance(BaseUnit.GetPosition(), _building.GetPosition()) < _building.GetAttackDistanceOffset() + 1.5f) {
                    // Дошли
                    BaseUnit.StopMoving();
                    BaseUnit.transform.LookAt(_building.GetPosition());
                    // Строим
                    _currentState = State.Constructing;

                }
                break;
            case State.Constructing:
                _constructionTimer -= Time.deltaTime;

                if (_constructionTimer < 0) {
                    float constructionTimerMax = 1f;
                    _constructionTimer += constructionTimerMax;

                    _buildingConstruction.AddProgress(1f);

                    // анимация
                    _unitVisualObject.Attack();

                    if (_buildingConstruction.IsConstructed()) {
                        BaseUnit.NormalMoveTo(BaseUnit.GetPosition());
                    }
                }
                break;
        }
    }
}
