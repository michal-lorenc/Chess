using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Pawn (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.PAWN;
    }

    public override void CalculateAttackedSquares()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackSquaresPawnStyle();
    }
}
