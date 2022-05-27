using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.KNIGHT;
    }

    public override void CalculateAttackedSquares()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackSquaresKnightStyle();
    }
}
