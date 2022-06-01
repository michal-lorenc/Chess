using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public Queen (PieceColor color, Vector2Int position, Chess chess) : base (color, position, chess)
    {
        Type = PieceType.QUEEN;
    }

    public override void CalculateAttackedSquares ()
    {
        base.CalculateAttackedSquares();
        
        SquareAttacker.AttackDiagonalSquares();
        SquareAttacker.AttackHorizontalSquares();
        SquareAttacker.AttackVerticalSquares();
    }
}
