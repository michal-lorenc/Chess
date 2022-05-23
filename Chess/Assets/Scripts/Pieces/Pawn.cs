using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Pawn (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.PAWN;
    }

    protected override List<Vector2Int> GetMoves()
    {
        return null;
    }
}
