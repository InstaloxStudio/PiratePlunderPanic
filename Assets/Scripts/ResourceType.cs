using UnityEngine;

[System.Serializable]
public class ResourceType : ScriptableObject
{
    public string ResourceName;
    public Sprite Sprite;
    public int MaxStack;
}
