using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight (PieceColor color, Vector2Int position, Chess chess) : base (color, position, chess)
    {
        Type = PieceType.KNIGHT;
    }

    public override void CalculateAttackedSquares()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackSquaresKnightStyle();
    }
}
