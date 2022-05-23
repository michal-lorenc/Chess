using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PieceUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPiece (Sprite sprite, Transform parentSquare)
    {
        spriteRenderer.sprite = sprite;
        transform.SetParent(parentSquare);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = Vector3.zero;
    }
}
