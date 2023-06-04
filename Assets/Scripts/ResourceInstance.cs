using UnityEngine;

public class ResourceInstance : MonoBehaviour
{
    public ResourceType Type;
    public int Amount;

    private void OnTriggerEnter(Collider other)
    {
        // On collision with player, add resource to player's inventory
    }
}
