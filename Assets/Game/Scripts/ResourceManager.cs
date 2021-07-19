using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event UnityAction OnResourceAmountChanged;

    [SerializeField] private ResourceTypeListSO _resourceTypeList;
    [SerializeField] private List<ResourceAmount> _startingResourceAmountList;

    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

    private void Awake() {
        Instance = this;

        _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        foreach (var resourceType in _resourceTypeList.List) {
            _resourceAmountDictionary[resourceType] = 0;
        }

        foreach (ResourceAmount resourceAmount in _startingResourceAmountList) {
            AddResourceAmount(resourceAmount.ResourceTypeSO, resourceAmount.Amount);
        }
    }

    public void AddResourceAmount(List<ResourceTypeSO> inventoryResourceTypeList) {
        foreach (ResourceTypeSO resourceTypeSO in inventoryResourceTypeList) {
            _resourceAmountDictionary[resourceTypeSO]++;
        }

        OnResourceAmountChanged?.Invoke();
    }

    public void AddResourceAmount(ResourceTypeSO resourceTypeSO, int amount) {
        _resourceAmountDictionary[resourceTypeSO] += amount;

        OnResourceAmountChanged?.Invoke();
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return _resourceAmountDictionary[resourceTypeSO];
    }

    public bool TrySpendResource(List<ResourceAmount> resourceAmountCostList) {
        bool canAfford = true;

        foreach (ResourceAmount resourceAmount in resourceAmountCostList) {
            if (GetResourceAmount(resourceAmount.ResourceTypeSO) >= resourceAmount.Amount) {
                // можем купить здание
            }
            else {
                // не можем
                canAfford = false;
                break;
            }
        }

        // если можем, тогда вычитаем ресурсы
        if (canAfford) {
            foreach (ResourceAmount resourceAmount in resourceAmountCostList) {
                _resourceAmountDictionary[resourceAmount.ResourceTypeSO] -= resourceAmount.Amount;
            }

            OnResourceAmountChanged?.Invoke();
            return true;
        }
        else {
            // не можем
            return false;
        }
    }
}

