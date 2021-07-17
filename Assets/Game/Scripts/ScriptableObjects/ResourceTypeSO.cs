using UnityEngine;

[CreateAssetMenu(fileName = "ResourceType", menuName = "Resources/ResourceType")]
public class ResourceTypeSO : ScriptableObject
{
    public Sprite sprite;
    public string type;
    public Color color;
}
