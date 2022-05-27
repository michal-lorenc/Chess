using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TileUI : MonoBehaviour
{
    public Vector2Int position;
    [SerializeField] private Image legalMoveMark;
    [SerializeField] private Image captureMoveMark;
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

    public void GetPiece ()
    {

    }

    public void SetPiece ()
    {

    }

    public void RemovePiece ()
    {

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
}
