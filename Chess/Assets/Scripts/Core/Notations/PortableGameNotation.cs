using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PortableGameNotation 
{
    public EventHandler<string> OnMoveHistoryUpdate;

    [SerializeField] private List<string> whiteMoves = new List<string>();
    [SerializeField] private List<string> blackMoves = new List<string>();

    private readonly Chess chess;

    public PortableGameNotation (List<string> whiteMoves, List<string> blackMoves)
    {
        this.whiteMoves = whiteMoves;
        this.blackMoves = blackMoves;
    }

    public PortableGameNotation (Chess chess)
    {
        this.chess = chess;
    }

    public string GetMovesHistory (bool displayNumbering, bool newLine = false)
    {
        string movesHistory = "";

        for (int i = 0; i < whiteMoves.Count; i++)
        {
            if (displayNumbering)
                movesHistory += $"{i + 1}. ";

            movesHistory += whiteMoves[i];

            if (i < blackMoves.Count)
            {
                movesHistory += " " + blackMoves[i];

                if (i != whiteMoves.Count - 1)
                {
                    if (newLine)
                        movesHistory += "\n";
                    else
                        movesHistory += " ";
                }
            }
            else
            {
                break;
            }
        }

        return movesHistory;
    }

    /// <summary>
    /// Converts move to algebraic notation. (this does not include check '+' & mate '#' symbols.)
    /// </summary>
    /// <returns>Algebraic notation string.</returns>
    public string ConvertMoveToAlgebraicNotation (Vector2Int from, Vector2Int to, char promotion = ' ')
    {
        string algebraicNotationString = "";

        Piece pieceToMove = chess.PiecesOnBoard[from.x, from.y];

        string destinationPosition = ChessHelper.Vector2IntPositionToStringPosition(to);
        bool doesTake = chess.PiecesOnBoard[to.x, to.y] != null;
        char pieceTypeChar = ChessHelper.PieceTypeToChar(pieceToMove.Type);

        if (pieceTypeChar == 'P')
        {
            if (doesTake)
                algebraicNotationString = ChessHelper.NumberToChar(from.x).ToString() + "x" + destinationPosition;
            else
                algebraicNotationString = destinationPosition;

            if (promotion != ' ')
                algebraicNotationString += "=" + promotion;

            return algebraicNotationString;
        }
        else if (pieceTypeChar == 'K')
        {
            int distance = Math.Abs(from.x - to.x);

            if (distance == 2)
            {
                if (from.x < to.x)
                    algebraicNotationString = "O-O";
                else
                    algebraicNotationString = "O-O-O";
            }
            else
            {
                algebraicNotationString += pieceTypeChar.ToString() + destinationPosition;
            }

            return algebraicNotationString;
        }
        else
        {
            algebraicNotationString += pieceTypeChar;

            // check if there is any other piece of the same color and type that could move to the same position
            List<Vector2Int> positionsOfOtherPiecesThatCanDoTheSameMove = new List<Vector2Int>();

            foreach (Piece piece in chess.PiecesOnBoard)
            {
                if (piece == null)
                    continue;

                if (piece.Color != pieceToMove.Color)
                    continue;

                if (piece.Type != pieceToMove.Type)
                    continue;

                if (piece == pieceToMove)
                    continue;

                if (piece.IsMoveLegal(to))
                    positionsOfOtherPiecesThatCanDoTheSameMove.Add(piece.Position);
            }

            // If any of those pieces are on the same Y, we need to clarify X position
            foreach (Vector2Int pos in positionsOfOtherPiecesThatCanDoTheSameMove)
            {
                if (pos.y == from.y)
                {
                    algebraicNotationString += ChessHelper.NumberToChar(from.x).ToString();
                    break;
                }
            }

            // If any of those pieces are on the same X, we need to clarify Y position
            foreach (Vector2Int pos in positionsOfOtherPiecesThatCanDoTheSameMove)
            {
                if (pos.x == from.x)
                {
                    algebraicNotationString += (from.x + 1).ToString();
                    break;
                }
            }

            if (doesTake)
                algebraicNotationString += "x";

            algebraicNotationString += destinationPosition;
            return algebraicNotationString;
        }
    }

    public void AddAlgebraicNotation (string algebraicNotation)
    {
        if (whiteMoves.Count == blackMoves.Count)
            whiteMoves.Add(algebraicNotation);
        else
            blackMoves.Add(algebraicNotation);

        // Event for UI
        OnMoveHistoryUpdate?.Invoke(this, GetMovesHistory(true, true));
    }

  /*  public static PortableGameNotation CreateFromString (string pgnString)
    {
        string[] pgnSplittedData = pgnString.Split(']');

        string[] moves = pgnSplittedData[pgnSplittedData.Length - 1].Split(' ');

        bool isWhiteMove = true;
        List<string> whiteMoves = new List<string>();
        List<string> blackMoves = new List<string>();

        foreach (string move in moves)
        {
            if (string.IsNullOrWhiteSpace(move))
                continue;

            if (move.Contains("."))
                continue;

            if (isWhiteMove)
                whiteMoves.Add(move);
            else
                blackMoves.Add(move);

            isWhiteMove = !isWhiteMove;
        }

        return new PortableGameNotation(whiteMoves, blackMoves);
    } */
}
