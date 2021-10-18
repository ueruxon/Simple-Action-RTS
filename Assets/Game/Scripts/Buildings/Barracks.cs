using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Barracks : MonoBehaviour
{
    private static List<Barracks> _instanceList;

    private Queue<UnitTypeSO> _unitList;
    private float _constructionTime;

    private UnitTypeSO _currentUnit;

    private void Awake() {
        if (_instanceList == null) {
            _instanceList = new List<Barracks>();
        }

        _instanceList.Add(this);
        _unitList = new Queue<UnitTypeSO>();
    }

    private void Update() {
        if (_unitList.Count > 0) {
            UnitCreation();
        }
    }

    public void AddUnitToQueue(UnitTypeSO unitTypeSO) {
        _unitList.Enqueue(unitTypeSO);
    }

    private void UnitCreation() {
        UnitTypeSO currentUnitData = _unitList.Peek();
        _currentUnit = currentUnitData;

        _constructionTime += Time.deltaTime;

        // создаем юнита
        if (_constructionTime >= currentUnitData.ConstructionTimerMax) {
            Transform currentUnit = Instantiate(currentUnitData.Template, transform.position, Quaternion.identity);

            Vector3 movePosition = transform.position + UtilsClass.GetRandomDirXZ() * Random.Range(2f, 5f);
            Unit unit = currentUnit.GetComponent<Unit>();
            UtilsClass.ActionNextFrame(() => {
                unit.NormalMoveTo(movePosition);
            });

            _unitList.Dequeue();
            _constructionTime = 0f;
        }
    }

    public Queue<UnitTypeSO> GetBarracksQueue() {
        return _unitList;
    }

    public float GetProgress() {
        return _constructionTime / _currentUnit.ConstructionTimerMax;
    }
}
