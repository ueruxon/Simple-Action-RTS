using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/ResourceTypeList")]
public class ResourceTypeListSO : ScriptableObject {
    public List<ResourceTypeSO> List;
    public ResourceTypeHolderSO ResourceHolderByType;

    [System.Serializable]
    public class ResourceTypeHolderSO {
        public ResourceTypeSO Wood;
        public ResourceTypeSO Stone;
        public ResourceTypeSO Iron;
    }
}
