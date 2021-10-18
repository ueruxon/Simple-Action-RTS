using UnityEngine;
using CodeMonkey.Utils;

[RequireComponent(typeof(Building))]
public class BuildingConstruction : MonoBehaviour
{
    // метод создания здания которое строится
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType) {
        // создаем здание в указанной точке
        GameObject buildingConstructionObject = Instantiate(buildingType.Template.gameObject, position, Quaternion.identity);
        // берем этот скрипт
        BuildingConstruction buildingConstruction = buildingConstructionObject.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingData(buildingType);

        return buildingConstruction;
    }

    private BuildingTypeSO _buildingTypeSO;
    private World_Bar _constructionBar;

    private float _progress;
    [HideInInspector] public bool IsBuilt = false;

    private void SetBuildingData(BuildingTypeSO buildingType) {
        _buildingTypeSO = buildingType;

        // создаем healtbar
        _constructionBar = World_Bar.Create(transform, new Vector3(0, 4, 0), new Vector3(3, .2f), Color.grey, Color.yellow, 1f, 0, new World_Bar.Outline { color = Color.black, size = .1f });
        LookAtCamera lookAtCamera = _constructionBar.GetGameObject().AddComponent<LookAtCamera>();
        lookAtCamera.SetInvert(true);

        _constructionBar.SetSize(0f);

        SetLayerRecursive(gameObject, 15);
    }

    public void AddProgress(float addAmount) {
        _progress += addAmount;
        _constructionBar.SetSize(_progress / _buildingTypeSO.ConstructionProgressMax);

        if (IsConstructed()) {
            // Завершаем строительство здания
            _constructionBar.SetActive(false);
            IsBuilt = true;
        }
    }

    public bool IsConstructed() {
        return _progress >= _buildingTypeSO.ConstructionProgressMax;
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer) {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform) {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    public void GetData() {
        print("в дате");
        //print(_buildingTypeSO.BuildingName);
        print(_progress);
        print("IsBuilt " + IsBuilt);
    }
}
