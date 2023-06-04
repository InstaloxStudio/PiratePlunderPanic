using System.Collections.Generic;

[System.Serializable]
public class Inventory
{
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
    }

    public bool HasResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            return false;

        return resources[type] >= amount;
    }

    // other inventory methods...
}
