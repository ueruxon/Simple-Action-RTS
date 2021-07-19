using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Buildings/BuildingType")]
public class BuildingTypeSO : ScriptableObject {

    public string BuildingName;
    public Transform Template;
    public Transform Visual;
    public Sprite Sprite;

    public float ConstructionProgressMax;
    public float ConstructionDistanceOffset;
    public List<ResourceAmount> ConstructionResourceAmountCostList;

    public string GetConstructionResourceString() {
        string str = "";

        foreach (ResourceAmount resourceAmount in ConstructionResourceAmountCostList) {
            str += "<color=#" + resourceAmount.ResourceTypeSO.ColorHex + ">" +
                resourceAmount.ResourceTypeSO.Type +
                resourceAmount.Amount + "</color> ";
        }

        return str;
    }

}
