using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook (PieceColor color, Vector2Int position) : base (color, position)
    {
        Type = PieceType.ROOK;
    }


    protected override List<Vector2Int> GetMoves ()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        for (int x = position.x + 1; x < Chess.boardSize; x++)
        {
            Piece piece = Chess.Singleton.PiecesOnBoard[x, position.y];

            if (piece == null)
            {
                possibleMoves.Add(new Vector2Int(x, position.y));
            }
            else
            {
                if (piece.Color == Color)
                    break;

                possibleMoves.Add(new Vector2Int(x, position.y));
                break;
            }
        }

        for (int x = position.x - 1; x >= 0; x--)
        {
            Piece piece = Chess.Singleton.PiecesOnBoard[x, position.y];

            if (piece == null)
            {
                possibleMoves.Add(new Vector2Int(x, position.y));
            }
            else
            {
                if (piece.Color == Color)
                    break;

                possibleMoves.Add(new Vector2Int(x, position.y));
                break;
            }
        }

        for (int y = position.y + 1; y < Chess.boardSize; y++)
        {
            Piece piece = Chess.Singleton.PiecesOnBoard[position.x, y];

            if (piece == null)
            {
                possibleMoves.Add(new Vector2Int(position.x, y));
            }
            else
            {
                if (piece.Color == Color)
                    break;

                possibleMoves.Add(new Vector2Int(position.x, y));
                break;
            }
        }

        for (int y = position.x - 1; y >= 0; y--)
        {
            Piece piece = Chess.Singleton.PiecesOnBoard[position.x, y];

            if (piece == null)
            {
                possibleMoves.Add(new Vector2Int(position.x, y));
            }
            else
            {
                if (piece.Color == Color)
                    break;

                possibleMoves.Add(new Vector2Int(position.x, y));
                break;
            }
        }

        return possibleMoves;
    }

    /// <summary>
    /// Calculates which square are attacked by this piece.
    /// It also calculates potentially attacked squares, so we know which pieces are pinned.
    /// </summary>
    public override void CalculateAttackedSquares ()
    {
        base.CalculateAttackedSquares();

        // Horizontal right side movement
        for (int x = position.x + 1; x < Chess.boardSize; x++)
        {
            if (!CheckSquare(x, position.y))
                break;
        }

        MergeAndClear();

        // Horizontal left side movement
        for (int x = position.x - 1; x >= 0; x--)
        {
            if (!CheckSquare(x, position.y))
                break;
        }

        MergeAndClear();

        // Vertical up side movement
        for (int y = position.y + 1; y < Chess.boardSize; y++)
        {
            if (!CheckSquare(position.x, y))
                break;
        }

        MergeAndClear();

        // Vertical down side movement
        for (int y = position.y - 1; y >= 0; y--)
        {
            if (!CheckSquare(position.x, y))
                break;
        }

        MergeAndClear();

        // diagonal up right
    //    for (int x = position.x + 1; x < Chess.boardSize; x++)
     //   {
      //      if (!CheckSquare(x, position.y + (x - position.x)))
     //           break;
     //   }
    }
}
