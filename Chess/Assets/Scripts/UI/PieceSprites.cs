using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PieceSprites
{
    [SerializeField] private Sprite king;
    [SerializeField] private Sprite queen;
    [SerializeField] private Sprite rook;
    [SerializeField] private Sprite bishop;
    [SerializeField] private Sprite knight;
    [SerializeField] private Sprite pawn;

    public Sprite GetSprite (PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.KING:
                return king;
            case PieceType.QUEEN:
                return queen;
            case PieceType.ROOK:
                return rook;
            case PieceType.BISHOP:
                return bishop;
            case PieceType.KNIGHT:
                return knight;
            case PieceType.PAWN:
                return pawn;
            default:
                throw new System.ArgumentException();
        }
    }
}
