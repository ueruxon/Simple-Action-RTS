using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class BuildingSelectionUI : MonoBehaviour
{
    [SerializeField] private Button _noneBtn;
    [SerializeField] private Button _storageBtn;
    [SerializeField] private Button _barracksBtn;

    [SerializeField] private BuildingTypeListSO _BuildingTypeListSO;

    private Dictionary<BuildingTypeSO, GameObject> _buildingTypeSelectedDictionary;

    private void Awake() {
        _buildingTypeSelectedDictionary = new Dictionary<BuildingTypeSO, GameObject>();

        _buildingTypeSelectedDictionary[_BuildingTypeListSO.BuildingHolderByType.None] = _noneBtn.transform.Find("Selected").gameObject;
        _buildingTypeSelectedDictionary[_BuildingTypeListSO.BuildingHolderByType.Storage] = _storageBtn.transform.Find("Selected").gameObject;
        _buildingTypeSelectedDictionary[_BuildingTypeListSO.BuildingHolderByType.Barracks] = _barracksBtn.transform.Find("Selected").gameObject;

        AddTooltip(_noneBtn.transform, "Курсор");
        AddTooltip(_storageBtn.transform, "Хранилище\n" +
            ResourceAmount.GetTooltipString(_BuildingTypeListSO.BuildingHolderByType.Storage.ConstructionResourceAmountCostList));
        AddTooltip(_barracksBtn.transform, "Барак\n" +
            ResourceAmount.GetTooltipString(_BuildingTypeListSO.BuildingHolderByType.Barracks.ConstructionResourceAmountCostList));
    }

    private void OnEnable() {
        _noneBtn.onClick.AddListener(() => BuildingPlacement.Instance.ClearBuildingType());
        _storageBtn.onClick.AddListener(() => BuildingPlacement.Instance.SetBuildingTypeSO(_BuildingTypeListSO.BuildingHolderByType.Storage));
        _barracksBtn.onClick.AddListener(() => BuildingPlacement.Instance.SetBuildingTypeSO(_BuildingTypeListSO.BuildingHolderByType.Barracks));
    }

    private void Start() {
        BuildingPlacement.Instance.OnBuildingTypeChanged += OnBuildingTypeChanged;
        UpdateBuildingSelected();
    }

    private void OnBuildingTypeChanged() {
        UpdateBuildingSelected();
    }

    private void UpdateBuildingSelected() {
        foreach (BuildingTypeSO buildingTypeSO in _buildingTypeSelectedDictionary.Keys) {
            _buildingTypeSelectedDictionary[buildingTypeSO].SetActive(false);
        }

        if (BuildingPlacement.Instance.GetBuildingTypeSO() == null) {
            _buildingTypeSelectedDictionary[_BuildingTypeListSO.BuildingHolderByType.None].SetActive(true);
        }
        else {
            _buildingTypeSelectedDictionary[BuildingPlacement.Instance.GetBuildingTypeSO()].SetActive(true);
        }
    }

    // codemonkey
    private void AddTooltip(Transform transform, string tooltipString) {
        transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => {
            TooltipCanvas.ShowTooltip_Static(tooltipString);
        };
        transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => {
            TooltipCanvas.HideTooltip_Static();
        };
    }
}
