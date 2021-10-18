using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildingPlacement : MonoBehaviour
{
    public static BuildingPlacement Instance { get; private set; }

    public event UnityAction OnBuildingTypeChanged;

    private BuildingTypeSO _currentBuildingTypeSO;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (_currentBuildingTypeSO != null) {
            // Если мы выбрали здание для постройки
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                Vector3 buildPosition = Mouse3D.GetMouseWorldPosition();
                if (ResourceManager.Instance.TrySpendResource(_currentBuildingTypeSO.ConstructionResourceAmountCostList)) {
                    // Тратим ресурсы и строим
                    BuildingConstruction.Create(buildPosition, _currentBuildingTypeSO);
                }
                else {
                    TooltipCanvas.ShowTooltip_Static("Не хватает средств на постройку!\n" +
                        ResourceAmount.GetTooltipString(_currentBuildingTypeSO.ConstructionResourceAmountCostList), 3f);
                }
            }

            if (Input.GetMouseButtonDown(1)) 
                SetBuildingTypeSO(null);
        }
    }

    public void SetBuildingTypeSO(BuildingTypeSO buildingTypeSO) {
        _currentBuildingTypeSO = buildingTypeSO;
        OnBuildingTypeChanged?.Invoke();
    }

    public void ClearBuildingType() {
        SetBuildingTypeSO(null);
    }

    public BuildingTypeSO GetBuildingTypeSO() {
        return _currentBuildingTypeSO;
    }
}
