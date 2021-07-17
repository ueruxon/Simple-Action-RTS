using UnityEngine;
using TMPro;

public class ResourceManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceAmountText;

    private void Awake() {
        // переделать 
        _resourceAmountText.SetText("0");
    }

    private void Start() {
        ResourceManager.Instance.OnResourceAmountChanged += OnResourceAmountChanged;
    }

    private void OnResourceAmountChanged() {
        // _resourceAmountText.SetText(ResourceManager.Instance.GetResourceAmount().ToString());
    }

    private void OnDisable() {
        ResourceManager.Instance.OnResourceAmountChanged -= OnResourceAmountChanged;
    }
}
