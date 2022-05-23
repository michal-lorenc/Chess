using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public Queen (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.QUEEN;
    }

    protected override List<Vector2Int> GetMoves()
    {
        return null;
    }
}
