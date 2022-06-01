using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Pawn (PieceColor color, Vector2Int position, Chess chess) : base (color, position, chess)
    {
        Type = PieceType.PAWN;
    }

    protected override List<Vector2Int> GetMoves ()
    {
        List<Vector2Int> finalMoves = new List<Vector2Int>();

        // Attacking moves validation
        foreach (Vector2Int move in base.GetMoves())
        {
            if (chess.PiecesOnBoard[move.x, move.y] == null)
                continue;

            if (chess.PiecesOnBoard[move.x, move.y].Color == Color)
                continue;

            finalMoves.Add(move);
        }

        // If pawn is placed on it's default position then it might go two squares forward
        int maxY = (Position.y == 1 && Color == PieceColor.WHITE) || (Position.y == 6 && Color == PieceColor.BLACK) ? 2 : 1;

        for (int y = 1; y <= maxY; y++)
        {
            int finalY = Position.y;

            // White pawns go upside and black go downside
            if (Color == PieceColor.WHITE)
                finalY += y;
            else
                finalY -= y;

            if (chess.PiecesOnBoard[Position.x, finalY] == null)
                finalMoves.Add(new Vector2Int(Position.x, finalY));
            else
                break;
        }

        return finalMoves;
    }

    public override void CalculateAttackedSquares()
    {
        base.CalculateAttackedSquares();

        SquareAttacker.AttackSquaresPawnStyle();
    }
}
