using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public Piece[,] PiecesOnBoard { get; private set; } = new Piece[boardSize, boardSize];
    public PortableGameNotation pgn;

    public static readonly int boardSize = 8;
    public static Chess Singleton { get; private set; }


    private void Awake ()
    {
        Singleton = this;
        PrepareBoard();

        PiecesOnBoard[0, 0].CalculateAttackedSquares();
        PiecesOnBoard[0, 0].GetLegalMoves();

        Piece piece = PiecesOnBoard[0, 0];

        foreach (Vector2Int pos in piece.GetLegalMoves())
        {
            Debug.Log("legal: " + pos);
        }
    }

    /// <summary>
    /// Looks through entire chessboard to find a king.
    /// </summary>
    /// <param name="kingColor">color of king</param>
    /// <returns>Reference to king of given color.</returns>
    public King GetKing (PieceColor kingColor)
    {
        foreach (Piece piece in PiecesOnBoard)
        {
            if (piece == null)
                continue;

            if (piece.Type == PieceType.KING && piece.Color == kingColor)
                return piece as King;
        }

        return null;
    }

    private void Start()
    {
        ExecuteMovePGN("Bf3", PieceColor.WHITE);
        ExecuteMovePGN("Kxg2", PieceColor.WHITE);
        ExecuteMovePGN("d4", PieceColor.WHITE);
        ExecuteMovePGN("Nxf2+", PieceColor.WHITE);

        pgn = PortableGameNotation.CreateFromString(@"[Event ""Live Chess""] [Site ""Chess.com""] [Date ""2022.05.16""] [Round "" ? ""] [White ""Sheri8cake""] [Black ""LoriMistrzuSzachista""] [Result ""1 - 0""] [ECO ""C55""] [WhiteElo ""643""] [BlackElo ""629""] [TimeControl ""180 + 2""] [EndTime ""19:41:42 PDT""] [Termination ""Sheri8cake won on time""] 1. e4 e5 2. Nf3 Nc6 3. Bc4 h6 4. d3 Nf6 5. Nc3 Bc5 6. d4 exd4 7. Nb5 a6 8. Bf4 axb5 9. Bxb5 Nxe4 10. O-O g5 11. Qe2 Qe7 12. a3 gxf4 13. b4 Bb6 14. c4 d6 15. c5 dxc5 16. bxc5 Bxc5 17. Qc4 Bd6 18. Nxd4 Nc5 19. Nxc6 bxc6 20. Bxc6+ Bd7 21. Bxa8 Kf8 22. Bf3 Be6 23. Qb5 Bd7 24. Qb8+ Qe8 25. Qa7 Rg8 26. g4 h5 27. h3 hxg4 28. hxg4 Bxg4 29. Bg2 Bh3 30. Rfe1 Rxg2+ 31. Kh1 Ne4 32. Qd4 f3 33. Qh8+ Ke7 34. Qxh3 Kf8 35. Qxf3 Nxf2+ 36. Kxg2 1-0");
    }

    private void PrepareBoard ()
    {
        LoadFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
    }

    private void ClearBoard ()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                PiecesOnBoard[x, y] = null;
            }
        }
    }

    private void LoadFromFEN (string fen)
    {
        ClearBoard();

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
                PiecesOnBoard[column, row] = PieceFactory.CreatePieceFromFEN(symbol, new Vector2Int(column, row));
                column++;
            }
        }
    }

    /// <summary>
    /// Converts algebraic chess notation to move.
    /// </summary>
    /// <param name="pgn"></param>
    /// <param name="color">color of the piece</param>
    private void ExecuteMovePGN (string pgn, PieceColor color)
    {
        PieceType pieceType = PieceType.PAWN;
        int fromDigit = -1;
        char fromChar = 'z';

        int destinationDigit = -1;
        char destinationChar = 'z';

        for (int i = 0; i < pgn.Length; i++)
        {
            if (char.IsDigit(pgn[i]))
            {
                if (fromDigit == -1 && fromChar == 'z')
                    fromDigit = int.Parse(pgn[i].ToString());

                destinationDigit = int.Parse(pgn[i].ToString());
            }
            else
            {
                if (pgn[i] == 'x' || pgn[i] == '+' || pgn[i] == '#')
                    continue;

                if (char.IsUpper(pgn[i]))
                {
                    pieceType = ChessHelper.CharToPieceType(pgn[i]);
                    continue;
                }

                if (fromDigit == -1 && fromChar == 'z')
                    fromChar = pgn[i];

                destinationChar = pgn[i];
            }
        }

        string destination = destinationChar.ToString() + destinationDigit.ToString();

        if (fromChar != 'z' && fromChar != destinationChar)
        {

        }
        else if (fromDigit != -1 && fromDigit != destinationDigit)
        {

        }
        else
        {

        }

        Debug.Log($"{pieceType} moved from {fromChar}, {fromDigit} to {destinationChar}, {destinationDigit}");
    }

    /// <summary>
    /// Supports two types of notation e.g:
    /// a1d8 and a1-d8
    /// </summary>
    /// <param name="moveString"></param>
    private void ExecuteMove (string moveString)
    {
        if (moveString.Length != 4)
            throw new System.ArgumentException();

        
    }

    private void ExecuteMove (Vector2Int from, Vector2Int to)
    {
        
    }
}
