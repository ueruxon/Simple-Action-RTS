using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : MonoBehaviour
{
    [SerializeField] private Transform _selectionAreaTransform = null;
    [SerializeField] private LayerMask _unitLayerMask;

    private List<Unit> _selectedUnitList;
    private Camera _mainCamera;

    private Vector3 _startSelectionAreaPosition;
    private Vector3 _selectionCenterPosition;

    [SerializeField] private Storage _storagePrefab;

    private void Awake() {
        _selectedUnitList = new List<Unit>();

        _mainCamera = Camera.main;

        _selectionAreaTransform.gameObject.SetActive(false);

        Application.targetFrameRate = 100;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _selectionAreaTransform.gameObject.SetActive(true);
            _startSelectionAreaPosition = Mouse3D.GetMouseWorldPosition();

            DeselectAllUnit();
        }

        // Область выделения юнитов
        if (Input.GetMouseButton(0)) {
            MakeSelectedArea();
        }

        if (Input.GetMouseButtonUp(0)) {
            _selectionAreaTransform.gameObject.SetActive(false);
            SelectUnitOnArea();
        }

        if (Input.GetMouseButtonDown(1)) {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit)) {

                // Дефолтное действие
                Action<Unit> unitAction = (Unit unit) => unit.NormalMoveTo(Mouse3D.GetMouseWorldPosition());

                // Майнинг
                if (raycastHit.collider.TryGetComponent(out ResourceNode resourceNode)) {
                    unitAction = (Unit unit) => unit.SetMiningResource(resourceNode);
                }

                // Атаковать
                if (raycastHit.collider.TryGetComponent(out Unit targetUnit)) {
                    if (targetUnit.IsEnemy()) {
                        unitAction = (Unit unit) => unit.SetTarget(targetUnit);    
                    }
                }

                // Выполняем экшен
                foreach (Unit unit in _selectedUnitList) {
                    if (unit.IsDead()) continue;
                    unitAction(unit);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Instantiate(_storagePrefab, new Vector3(-12.78f, 0, 1.33f), Quaternion.identity);
        }
    }

    private void MakeSelectedArea() {
        Vector3 currentMoustPosition = Mouse3D.GetMouseWorldPosition();
        // нижний левый угол (начало)
        Vector3 lowerLeft = new Vector3(
            Mathf.Min(_startSelectionAreaPosition.x, currentMoustPosition.x),
            0,
            Mathf.Min(_startSelectionAreaPosition.z, currentMoustPosition.z)
        );
        // правый верхний
        Vector3 upperRight = new Vector3(
            Mathf.Max(_startSelectionAreaPosition.x, currentMoustPosition.x),
            0,
            Mathf.Max(_startSelectionAreaPosition.z, currentMoustPosition.z)
        );

        // центр зоны выделения
        _selectionCenterPosition = new Vector3(
            lowerLeft.x + ((upperRight.x - lowerLeft.x) / 2f),
            0,
            lowerLeft.z + ((upperRight.z - lowerLeft.z) / 2f)
        );

        _selectionAreaTransform.position = lowerLeft;
        _selectionAreaTransform.localScale = upperRight - lowerLeft;
    }

    private void SelectUnitOnArea() {
        Vector3 halfExtents = new Vector3(_selectionAreaTransform.localScale.x / 2f, 1f, _selectionAreaTransform.localScale.z / 2f);

        bool isSingleTarget = false;
        // минимальные размеры выделения (если цель одна)
        if (halfExtents.x < .5f) {
            halfExtents.x = .35f;
            isSingleTarget = true;
        }
        if (halfExtents.z < .5f) {
            halfExtents.z = .35f;
            isSingleTarget = true;
        }

        Collider[] hitColliders = Physics.OverlapBox(_selectionCenterPosition, halfExtents, Quaternion.identity, _unitLayerMask);

        foreach (var collider in hitColliders) {
            if (collider.TryGetComponent(out Unit unit)) {
                if (!unit.IsEnemy()) {
                    unit.SetSelected(true);
                    _selectedUnitList.Add(unit);
                
                    if (isSingleTarget) {
                        break;
                    }
                }
            }
        }
    }

    void OnDrawGizmos() {
        Vector3 halfExtents = new Vector3(_selectionAreaTransform.localScale.x, 1f, _selectionAreaTransform.localScale.z);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_selectionCenterPosition, halfExtents);
    }

    private void DeselectAllUnit() {
        foreach (var unit in _selectedUnitList) {
            unit.SetSelected(false);
        }
        // удалить
        _selectedUnitList.Clear();
    }
}
