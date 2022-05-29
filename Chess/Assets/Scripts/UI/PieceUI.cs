using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class PieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    private TileUI parentTile;

    private void Awake ()
    {
        image = GetComponent<Image>();
    }

    public PieceUI SetPiece (Sprite sprite)
    {
        image.sprite = sprite;

        return this;
    }

    public PieceUI SetParent (TileUI parent)
    {
        parentTile = parent;
        transform.SetParent(parentTile.transform);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = Vector3.zero;

        return this;
    }

    public TileUI GetParent ()
    {
        return parentTile;
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        transform.SetParent(parentTile.transform.parent.parent);
        transform.position = eventData.position;
        ChessUI.Singleton.draggedPiece = this;
        image.raycastTarget = false;
    }

    public void OnDrag (PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        SetParent(parentTile);
        ChessUI.Singleton.draggedPiece = null;
        image.raycastTarget = true;
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        transform.SetParent(parentTile.transform.parent.parent);
        transform.position = eventData.position;
        ChessUI.Singleton.ShowLegalMoves(parentTile.position);
       // ChessUI.Singleton.draggedPiece = this;
        ChessUI.Singleton.selectedPiece = this;
        image.raycastTarget = false;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        SetParent(parentTile);
        parentTile.SetOutline(false);
       // ChessUI.Singleton.draggedPiece = null;
        image.raycastTarget = true;
    }
}
