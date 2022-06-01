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

        string[] fenSplit = fen.Split(' ');
        int row = 7, column = 0;

        foreach (char symbol in fenSplit[0])
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
                chess.PiecesOnBoard[column, row] = PieceFactory.CreatePieceFromFEN(symbol, new Vector2Int(column, row), chess);
                column++;
            }
        }

        if (fenSplit.Length > 1)
        {
            chess.ColorToMove = ChessHelper.CharToPieceColor(Convert.ToChar(fenSplit[1]));
        }

        if (fenSplit.Length > 2)
        {
            King whiteKing = chess.GetKing(PieceColor.WHITE);
            King blackKing = chess.GetKing(PieceColor.BLACK);
            string castlingInfo = fenSplit[2];

            if (castlingInfo == "-")
            {
                whiteKing.CanCastleKingSide = false;
                whiteKing.CanCastleQueenSide = false;
                blackKing.CanCastleKingSide = false;
                blackKing.CanCastleQueenSide = false;
            }
            else
            {
                if (castlingInfo.Contains("K"))
                    whiteKing.CanCastleKingSide = true;
                else
                    whiteKing.CanCastleKingSide = false;

                if (castlingInfo.Contains("Q"))
                    whiteKing.CanCastleQueenSide = true;
                else
                    whiteKing.CanCastleQueenSide = false;

                if (castlingInfo.Contains("k"))
                    blackKing.CanCastleKingSide = true;
                else
                    blackKing.CanCastleKingSide = false;

                if (castlingInfo.Contains("q"))
                    blackKing.CanCastleQueenSide = true;
                else
                    blackKing.CanCastleQueenSide = false;
            }
        }

        if (fenSplit.Length > 4)
        {
            chess.MovesWithoutCaptureCount = Convert.ToInt32(fenSplit[4]);
        }

        if (fenSplit.Length > 5)
        {
            chess.TotalMovesCount = Convert.ToInt32(fenSplit[5]);
        }
    }

    /// <summary>
    /// Converts current game state to FEN string.
    /// </summary>
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

        finalFEN += $" {ChessHelper.PieceColorToChar(chess.ColorToMove)} {GetCastlingInfo()} - {chess.MovesWithoutCaptureCount} {chess.TotalMovesCount}";

        return finalFEN;
    }

    /// <summary>
    /// Returns information about possible castling moves
    /// </summary>
    private string GetCastlingInfo ()
    {
        King whiteKing = chess.GetKing(PieceColor.WHITE);
        King blackKing = chess.GetKing(PieceColor.BLACK);

        string castlingInfo = "";

        if (whiteKing.CanCastleKingSide)
            castlingInfo += "K";
        if (whiteKing.CanCastleQueenSide)
            castlingInfo += "Q";
        if (blackKing.CanCastleKingSide)
            castlingInfo += "k";
        if (blackKing.CanCastleQueenSide)
            castlingInfo += "q";

        if (string.IsNullOrEmpty(castlingInfo))
            return "-";

        return castlingInfo;
    }
}
