using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook (PieceColor color, Vector2Int position, Chess chess) : base (color, position, chess)
    {
        Type = PieceType.ROOK;
    }

    public override void CalculateAttackedSquares ()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackHorizontalSquares();
        SquareAttacker.AttackVerticalSquares();
    }
}
