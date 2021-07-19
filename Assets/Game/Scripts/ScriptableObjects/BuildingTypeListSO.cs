using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/BuildingTypeList")]
public class BuildingTypeListSO : ScriptableObject
{
    public List<BuildingTypeSO> List;
    public BuildingTypeHolderSO BuildingHolderByType;

    [System.Serializable]
    public class BuildingTypeHolderSO {
        public BuildingTypeSO None;
        public BuildingTypeSO Storage;
        public BuildingTypeSO Barracks;
    }
}
