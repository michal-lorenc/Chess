using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.KNIGHT;
    }

    protected override List<Vector2Int> GetMoves()
    {
        return null;
    }
}
