using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResourceManagerUI : MonoBehaviour
{
    [SerializeField] private ResourceTypeListSO _resourceTypeList;
    [SerializeField] private TMP_Text _woodText;
    [SerializeField] private TMP_Text _stoneText;
    [SerializeField] private TMP_Text _ironText;


    private Dictionary<ResourceTypeSO, TMP_Text> _resourceTextDictionary;

    private void Awake() {
        _resourceTextDictionary = new Dictionary<ResourceTypeSO, TMP_Text>();

        _resourceTextDictionary[_resourceTypeList.ResourceHolderByType.Wood] = _woodText;
        _resourceTextDictionary[_resourceTypeList.ResourceHolderByType.Stone] = _stoneText;
        _resourceTextDictionary[_resourceTypeList.ResourceHolderByType.Iron] = _ironText;
    }

    private void Start() {
        ResourceManager.Instance.OnResourceAmountChanged += OnResourceAmountChanged;

        OnResourceAmountChanged();
    }

    private void OnResourceAmountChanged() {
        UpdateResourceAmounts();
    }

    private void UpdateResourceAmounts() {
        foreach (ResourceTypeSO resourceTypeSO in _resourceTypeList.List) {
            // берем текущее кол-во ресурса
            string resourceAmountText = ResourceManager.Instance.GetResourceAmount(resourceTypeSO).ToString();
            _resourceTextDictionary[resourceTypeSO].SetText(resourceAmountText);
        }
    }

    private void OnDisable() {
        ResourceManager.Instance.OnResourceAmountChanged -= OnResourceAmountChanged;
    }
}
