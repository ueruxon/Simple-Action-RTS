using System;
using UnityEngine;

public class UnitsController : MonoBehaviour
{
    [SerializeField] private UnitsSelectionManager _unitsSelectionManager;

    private Camera _mainCamera;

    private void Awake() {
        _mainCamera = Camera.main;

        Application.targetFrameRate = 100;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit)) {

                // Дефолтное действие
                Action<Unit> unitAction = (Unit unit) => unit.NormalMoveTo(Mouse3D.GetMouseWorldPosition());

                // Майнинг
                if (raycastHit.collider.TryGetComponent(out ResourceNode resourceNode)) {
                    unitAction = (Unit unit) => unit.SetMiningResource(resourceNode);
                }
                // Строительство
                if (raycastHit.collider.TryGetComponent(out BuildingConstruction buildingConstruction)) {
                    if (!buildingConstruction.IsConstructed()) {
                        unitAction = (Unit unit) => unit.SetConstructionBuilding(buildingConstruction);
                    }
                }
                // Атаковать
                if (raycastHit.collider.TryGetComponent(out Unit targetUnit)) {
                    if (targetUnit.IsEnemy()) {
                        unitAction = (Unit unit) => unit.SetTarget(targetUnit);    
                    }
                }

                // Выполняем экшен
                foreach (Unit unit in _unitsSelectionManager.GetSelectionUnitList()) {
                    if (unit.IsDead()) continue;
                    unitAction(unit);
                }
            }
        }
    }
}
