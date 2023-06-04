using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDatabase", menuName = "Resources/Database")]
public class ResourceDatabase : ScriptableObject
{
    public Dictionary<string, ResourceType> resourceDictionary = new Dictionary<string, ResourceType>();
}
