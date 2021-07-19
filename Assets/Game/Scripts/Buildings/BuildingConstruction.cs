using UnityEngine;
using CodeMonkey.Utils;

public class BuildingConstruction : MonoBehaviour
{
    // метод создания здания которое строится
    public static BuildingConstruction Create(GameObject buildingTemplate, Vector3 position, BuildingTypeSO buildingType) {
        // создаем здание в указанной точке
        GameObject buildingConstructionObject = Instantiate(buildingTemplate, position, Quaternion.identity);
        // берем этот скрипт
        BuildingConstruction buildingConstruction = buildingConstructionObject.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }

    private BuildingTypeSO _buildingTypeSO;
    private World_Bar _constructionBar;
    private float _progress;

    private void SetBuildingType(BuildingTypeSO buildingType) {
        _buildingTypeSO = buildingType;

        // создаем healtbar
        _constructionBar = World_Bar.Create(transform, new Vector3(0, 4, 0), new Vector3(3, .2f), Color.grey, Color.yellow, 1f, 0, new World_Bar.Outline { color = Color.black, size = .1f });
        LookAtCamera lookAtCamera = _constructionBar.GetGameObject().AddComponent<LookAtCamera>();
        lookAtCamera.SetInvert(true);

        _constructionBar.SetSize(0f);

        // создаем префаб (будем двигать его за мышкой)
        Transform buildingTransform = Instantiate(buildingType.Template);
        buildingTransform.SetParent(transform);
        buildingTransform.localPosition = Vector3.zero;
        buildingTransform.eulerAngles = Vector3.zero;

        SetLayerRecursive(buildingTransform.gameObject, 15);
    }

    public void AddProgress(float addAmount) {
        _progress += addAmount;

        _constructionBar.SetSize(_progress / _buildingTypeSO.ConstructionProgressMax);

        if (IsConstructed()) {
            // строим здание
            Instantiate(_buildingTypeSO.Template, transform.position, Quaternion.identity);
            Destroy(gameObject);
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
}
