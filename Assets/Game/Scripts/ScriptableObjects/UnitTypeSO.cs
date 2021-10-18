using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitType", menuName = "Units/UnitType")]
public class UnitTypeSO : ScriptableObject
{
    public Transform Template;
    public Sprite Sprite;

    public float ConstructionTimerMax;
    public List<ResourceAmount> ConstructionResourceAmountCostList;
}
