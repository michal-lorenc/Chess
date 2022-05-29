using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TileUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector2Int position;
    [SerializeField] private Image legalMoveMark;
    [SerializeField] private Image captureMoveMark;
    [SerializeField] private Image outlineImage;
    private Image backgroundImage;

    private void Awake ()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void SetColor (Color color)
    {
        backgroundImage.color = color;
    }

    public void SetSprite (Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public PieceUI GetPiece ()
    {
        // Get component is slow, but it's quick workaround for now
        return GetComponentInChildren<PieceUI>();
    }

    public void SetOutline (bool visible)
    {
        outlineImage.gameObject.SetActive(visible);
    }


    public void ShowLegalMoveMark ()
    {
        legalMoveMark.gameObject.SetActive(true);
    }

    public void HideLegalMoveMark()
    {
        legalMoveMark.gameObject.SetActive(false);
        captureMoveMark.gameObject.SetActive(false);
    }

    public void OnDrop (PointerEventData eventData)
    {
        PieceUI draggedPiece = ChessUI.Singleton.draggedPiece;

        if (draggedPiece != null)
        {
            Debug.Log("dragged piece is not null");

            bool success = Chess.Singleton.ExecuteMove(draggedPiece.GetParent().position, position);

            if (success)
            {
                Debug.Log("Move OK. " + draggedPiece.GetParent().position + " : " + position);

                PieceUI pieceOnThisSlot = GetPiece();
                if (pieceOnThisSlot != null)
                    Destroy(pieceOnThisSlot.gameObject);


                draggedPiece.SetParent(this);
                ChessUI.Singleton.HideLegalMoves();
            }
            else
            {
                Debug.Log("Move failed. " + draggedPiece.GetParent().position + " : " + position);
            }
        }

        outlineImage.gameObject.SetActive(false);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        outlineImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (ChessUI.Singleton.draggedPiece != null)
        {
            outlineImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        ChessUI.Singleton.HideLegalMoves();
        ChessUI.Singleton.selectedPiece = null;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        Debug.Log("Pointer up");
    }
}
