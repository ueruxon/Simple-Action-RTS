using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour {
    public static BuildingSelectionManager Instance { get; private set; }

    private Camera _mainCamera;

    private void Awake() {
        Instance = this;

        _mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit)) {
                if (raycastHit.collider.TryGetComponent(out Barracks barracks)) {
                    if (barracks.GetComponent<BuildingConstruction>().IsBuilt) {
                        BarracksUI.Instance.Show(barracks);
                    }
                }
            }
        }
    }

}
