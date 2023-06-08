using UnityEngine;

public class ResourceInstance : MonoBehaviour,IInteractable
{
    public ResourceType Type;
    public int Amount;

    public void Highlight(PlayerCharacter interactor)
    {

    }

    public virtual void Interact(PlayerCharacter interactor)
    {

    }

    public void UnHighlight(PlayerCharacter interactor)
    {

    }
}
