using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForsythEdwardsNotation
{
    private readonly Chess chess;

    public ForsythEdwardsNotation (Chess chess)
    {
        this.chess = chess;
    }

    /// <summary>
    /// Converts FEN to pieces on board.
    /// </summary>
    /// <param name="fen">FEN (start position by default)</param>
    public void ConvertFenToChessboard (string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")
    {
        chess.ClearBoard();

        fen = fen.Split(' ')[0];
        int row = 7, column = 0;

        foreach (char symbol in fen)
        {
            if (symbol == '/')
            {
                row--;
                column = 0;
                continue;
            }

            if (char.IsDigit(symbol))
            {
                column += int.Parse(symbol.ToString());
            }
            else
            {
                chess.PiecesOnBoard[column, row] = PieceFactory.CreatePieceFromFEN(symbol, new Vector2Int(column, row));
                column++;
            }
        }
    }

    public string ConvertChessboardToFEN ()
    {
        string finalFEN = "";

        for (int y = Chess.boardSize - 1; y >= 0; y--)
        {
            int emptySpaceCount = 0;

            for (int x = 0; x < Chess.boardSize; x++)
            {
                if (chess.PiecesOnBoard[x, y] == null)
                {
                    emptySpaceCount++;

                    if (emptySpaceCount > 1)
                    {
                        finalFEN = finalFEN.Remove(finalFEN.Length - 1);
                        finalFEN += emptySpaceCount.ToString();
                    }
                    else
                    {
                        finalFEN += emptySpaceCount.ToString();
                    }

                    continue;
                }

                emptySpaceCount = 0;

                if (chess.PiecesOnBoard[x, y].Color == PieceColor.BLACK)
                {
                    finalFEN += char.ToLower(ChessHelper.PieceTypeToChar(chess.PiecesOnBoard[x, y].Type));
                }
                else
                {
                    finalFEN += ChessHelper.PieceTypeToChar(chess.PiecesOnBoard[x, y].Type);
                }
            }

            if (y != 0)
                finalFEN += '/';
        }



        return finalFEN;
    }
}
