using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool IsMated { get; private set; }
    public bool IsChecked { get { return CheckingPieces.Count > 0; } }
    public bool IsDoubleChecked { get { return CheckingPieces.Count > 1; } }

    /// <summary>
    /// Reference to pieces by which the king is in check.
    /// </summary>
    public List<Piece> CheckingPieces { get; set; } = new List<Piece>();

    public King (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.KING;
    }

    public override void CalculateAttackedSquares ()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackNeighbourSquares();
    }
}
