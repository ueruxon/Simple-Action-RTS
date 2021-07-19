using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementGhost : MonoBehaviour
{
    private Transform _visual;

    private void Start() {
        RefreshVisual();

        BuildingPlacement.Instance.OnBuildingTypeChanged += OnBuildingTypeChanged;
    }

    private void OnBuildingTypeChanged() {
        RefreshVisual();
    }

    private void LateUpdate() {
        Vector3 targetPosition = Mouse3D.GetMouseWorldPosition();
        targetPosition.y = .3f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 30f);    
    }

    private void RefreshVisual() {
        if (_visual != null) {
            Destroy(_visual.gameObject);
            _visual = null;
        }

        BuildingTypeSO buildingTypeSO = BuildingPlacement.Instance.GetBuildingTypeSO();

        if (buildingTypeSO != null) {
            _visual = Instantiate(buildingTypeSO.Visual, Vector3.zero, Quaternion.identity);
            _visual.parent = transform;
            _visual.localPosition = Vector3.zero;
            _visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(_visual.gameObject, 15);
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer) {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform) {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
