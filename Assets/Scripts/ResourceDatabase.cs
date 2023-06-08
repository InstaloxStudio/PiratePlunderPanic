using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDatabase", menuName = "Resources/Database")]
public class ResourceDatabase : SerializedScriptableObject
{
    public Dictionary<string, ResourceType> resourceDictionary = new Dictionary<string, ResourceType>();
}
