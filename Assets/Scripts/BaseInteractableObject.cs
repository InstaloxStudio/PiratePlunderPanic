using UnityEngine.EventSystems;
using Mirror;
using UnityEngine;

public class BaseInteractableObject : NetworkBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color _originalColor=Color.white;
    public Color _highlightColor=Color.white;
    public bool _canInteract = true;
    public bool _canHighlight = true;

    public Material _material;
    private void Awake()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
        _originalColor = _material.color;
    }
   

    public virtual void Highlight(PlayerCharacter interactor)
    {
        _material?.SetColor("_BaseColor", _highlightColor);
    }

    public virtual void Interact(PlayerCharacter interactor)
    {
       // Debug.Log(interactor.name + " interacting with " + gameObject.name);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Highlight(null);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UnHighlight(null);
    }

    public virtual void UnHighlight(PlayerCharacter interactor)
    {
        _material?.SetColor("_BaseColor", _originalColor);

    }
}
