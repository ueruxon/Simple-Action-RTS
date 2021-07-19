using UnityEngine;

[CreateAssetMenu(fileName = "ResourceType", menuName = "Resources/ResourceType")]
public class ResourceTypeSO : ScriptableObject
{
    public Sprite Sprite;
    public string Type;
    public Color Color;
    public string ColorHex;
}
