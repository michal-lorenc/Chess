using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class PieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    private Transform parent;

    private void Awake ()
    {
        image = GetComponent<Image>();
    }

    public void SetPiece (Sprite sprite, Transform parentSquare)
    {
        parent = parentSquare;
        image.sprite = sprite;
        transform.SetParent(parentSquare);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        transform.SetParent(parent.parent.parent);
        transform.position = eventData.position;
    }

    public void OnDrag (PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        transform.SetParent(parent.parent.parent);
        transform.position = eventData.position;
        ChessUI.Singleton.ShowLegalMoves(parent.GetComponent<TileUI>().position);
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }
}
