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

        //foreach (ResourceAmount resourceAmount in _startingResourceAmountList) {
        //    AddResourceAmount(resourceAmount.ResourceTypeSO, resourceAmount.Amount);
        //}
    }

    public void AddResourceAmount(List<ResourceTypeSO> inventoryResourceTypeList) {
        foreach (ResourceTypeSO resourceTypeSO in inventoryResourceTypeList) {
            _resourceAmountDictionary[resourceTypeSO]++;
        }

        foreach (KeyValuePair<ResourceTypeSO, int> keyValue in _resourceAmountDictionary) {
            Debug.Log(keyValue.Key + " - " + keyValue.Value);
        }

        //OnResourceAmountChanged?.Invoke();
    }

    public void AddResourceAmount(ResourceTypeSO resourceTypeSO, int amount) {
        _resourceAmountDictionary[resourceTypeSO] += amount;

        //OnResourceAmountChanged?.Invoke();

        foreach (KeyValuePair<ResourceTypeSO, int> keyValue in _resourceAmountDictionary) {
            Debug.Log(keyValue.Key + " - " + keyValue.Value);
        }
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return _resourceAmountDictionary[resourceTypeSO];
    }
}

