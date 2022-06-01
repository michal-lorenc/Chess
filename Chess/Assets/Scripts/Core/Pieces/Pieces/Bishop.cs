using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop (PieceColor color, Vector2Int position, Chess chess) : base (color, position, chess)
    {
        Type = PieceType.BISHOP;
    }

    public override void CalculateAttackedSquares()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackDiagonalSquares();
    }
}
