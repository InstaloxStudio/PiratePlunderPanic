using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
public class CollectableObject : NetworkBehaviour , ICollectable, IInteractable,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public ResourceType Type;
    public int Amount;

    public virtual void Collect(PlayerCharacter interactor)
    {

    }

    public void Highlight(PlayerCharacter interactor)
    {

    }

    public void Interact(PlayerCharacter interactor)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void UnHighlight(PlayerCharacter interactor)
    {

    }
}
