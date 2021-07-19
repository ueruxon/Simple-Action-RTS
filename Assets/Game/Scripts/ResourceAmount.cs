using System.Collections.Generic;

[System.Serializable]
public class ResourceAmount {
    public ResourceTypeSO ResourceTypeSO;
    public int Amount;

    public static string GetTooltipString(List<ResourceAmount> resourceAmountList) {
        string tooltipString = "";

        foreach (ResourceAmount resourceAmount in resourceAmountList) {
            tooltipString += "<color=#" + resourceAmount.ResourceTypeSO.ColorHex + ">" +
                resourceAmount.ResourceTypeSO.Type + resourceAmount.Amount + "</color> ";
        }

        return tooltipString;
    }
}
