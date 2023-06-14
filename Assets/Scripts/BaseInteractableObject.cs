using UnityEngine.EventSystems;
using Mirror;
using UnityEngine;

public class BaseInteractableObject : NetworkBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Color _originalColor=Color.white;
    public Color _highlightColor=Color.white;
    public bool _canInteract = true;
    public bool _canHighlight = true;


    private void Awake()
    {

    }

    public virtual void Highlight(PlayerCharacter interactor)
    {

    }

    public virtual void Interact(PlayerCharacter interactor)
    {
        Debug.Log(interactor.name + " interacting with " + gameObject.name);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public virtual void UnHighlight(PlayerCharacter interactor)
    {

    }
}
