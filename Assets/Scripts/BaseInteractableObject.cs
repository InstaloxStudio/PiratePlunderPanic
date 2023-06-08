using UnityEngine.EventSystems;
using Mirror;
using UnityEngine;
public class BaseInteractableObject : NetworkBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    public Color _highlightColor;
    public bool _canInteract = true;
    public bool _canHighlight = true;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        _originalColor = _spriteRenderer.color;
    }

    public virtual void Highlight(PlayerCharacter interactor)
    {
        _spriteRenderer.color = _highlightColor;

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
        _spriteRenderer.color = _highlightColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = _originalColor;
    }

    public virtual void UnHighlight(PlayerCharacter interactor)
    {
        _spriteRenderer.color = _originalColor;

    }
}
