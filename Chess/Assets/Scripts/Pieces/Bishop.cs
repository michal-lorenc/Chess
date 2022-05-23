using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.BISHOP;
    }

    protected override List<Vector2Int> GetMoves()
    {
        return null;
    }
}
