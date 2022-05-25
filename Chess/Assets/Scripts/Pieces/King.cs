using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool IsMated { get; }
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

    public override List<Vector2Int> CalculateLegalMoves ()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (Vector2Int position in GetMoves())
        {
            if (Chess.Singleton.IsSquareAttacked(position, Color == PieceColor.WHITE ? PieceColor.BLACK : PieceColor.WHITE))
                continue;

            positions.Add(position);
        }

        legalMovesPositions = positions;
        return positions;
    }

    public override void CalculateAttackedSquares ()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackNeighbourSquares();
    }
}
