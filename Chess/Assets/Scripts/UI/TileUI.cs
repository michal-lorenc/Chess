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
    [SerializeField] private Image highlightImage;
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

    public void SetHighlightColor (Color color)
    {
        highlightImage.color = color;
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

    public void SetHighlight (bool visible)
    {
        highlightImage.gameObject.SetActive(visible);
    }


    public void ShowLegalMoveMark ()
    {
        if (GetPiece() != null)
            captureMoveMark.gameObject.SetActive(true);
        else
            legalMoveMark.gameObject.SetActive(true);
    }

    public void HideLegalMoveMark ()
    {
        legalMoveMark.gameObject.SetActive(false);
        captureMoveMark.gameObject.SetActive(false);
    }

    public void OnDrop (PointerEventData eventData)
    {
        PieceUI draggedPiece = ChessUI.Singleton.draggedPiece;
        TryMovePiece(draggedPiece);

        outlineImage.gameObject.SetActive(false);
    }

    private void TryMovePiece (PieceUI pieceUI)
    {
        if (pieceUI == null)
            return;

        bool success = ChessUI.Singleton.Chess.ExecuteMove(pieceUI.GetParent().position, position);

        if (success)
        {
            Debug.Log("Move OK. " + pieceUI.GetParent().position + " : " + position);

            PieceUI pieceOnThisSlot = GetPiece();
            if (pieceOnThisSlot != null)
                Destroy(pieceOnThisSlot.gameObject);

            pieceUI.SetParent(this);
            ChessUI.Singleton.HideLegalMoves();
        }
        else
        {
            Debug.Log("Move failed. " + pieceUI.GetParent().position + " : " + position);
        }
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
        PieceUI selectedPiece = ChessUI.Singleton.selectedPiece;
        TryMovePiece(selectedPiece);

        ChessUI.Singleton.HideLegalMoves();
        ChessUI.Singleton.selectedPiece = null;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        Debug.Log("Pointer up");
    }
}
