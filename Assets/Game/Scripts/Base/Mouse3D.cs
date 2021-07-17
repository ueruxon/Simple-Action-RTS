using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    public static Mouse3D Instance { get; private set; }
    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    [SerializeField] private LayerMask _mouseColliderLayerMask = new LayerMask();

    private Camera _mainCamera;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _mainCamera = Camera.main;
    }

    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _mouseColliderLayerMask)) {
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }
}
