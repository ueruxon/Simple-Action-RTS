using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/ResourceTypeList")]
public class ResourceTypeListSO : ScriptableObject {
    public List<ResourceTypeSO> List;
}
