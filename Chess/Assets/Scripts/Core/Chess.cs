using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chess
{
    public Piece[,] PiecesOnBoard { get; private set; } = new Piece[boardSize, boardSize];
    public PortableGameNotation PGN { get; private set; }
    public ForsythEdwardsNotation FEN { get; private set; }
    public UniversalChessInterface UCI { get; private set; }
    public OpeningsBook OpeningsBook { get; private set; }


    public int TotalMovesCount { get; set; } = 0;
    public int MovesWithoutCaptureCount { get; set; } = 0;
    public PieceColor ColorToMove { get; set; } = PieceColor.WHITE;

    public static readonly int boardSize = 8;

    public EventHandler<Move> OnPieceMoved;
    public EventHandler<GameResult> OnGameEnded;

    // TODO
    //  - Castling
    //  - En passant
    //  - Pawn Promotion
    //  - 50 move rule
    //  - 3x repeated position rule

    public Chess ()
    {
        PGN = new PortableGameNotation(this);
        FEN = new ForsythEdwardsNotation(this);
        UCI = new UniversalChessInterface(this);
        OpeningsBook = new OpeningsBook();

        Debug.Log($"There are {OpeningsBook.GetOpeningsAmount()} openings loaded.");

        PrepareBoard();
    }

   /*
    private void Start()
    {
        ExecuteMovePGN("Bf3", PieceColor.WHITE);
        ExecuteMovePGN("Kxg2", PieceColor.WHITE);
        ExecuteMovePGN("d4", PieceColor.WHITE);
        ExecuteMovePGN("Nxf2+", PieceColor.WHITE);

        //  PGN = PortableGameNotation.CreateFromString(@"[Event ""Live Chess""] [Site ""Chess.com""] [Date ""2022.05.16""] [Round "" ? ""] [White ""Sheri8cake""] [Black ""LoriMistrzuSzachista""] [Result ""1 - 0""] [ECO ""C55""] [WhiteElo ""643""] [BlackElo ""629""] [TimeControl ""180 + 2""] [EndTime ""19:41:42 PDT""] [Termination ""Sheri8cake won on time""] 1. e4 e5 2. Nf3 Nc6 3. Bc4 h6 4. d3 Nf6 5. Nc3 Bc5 6. d4 exd4 7. Nb5 a6 8. Bf4 axb5 9. Bxb5 Nxe4 10. O-O g5 11. Qe2 Qe7 12. a3 gxf4 13. b4 Bb6 14. c4 d6 15. c5 dxc5 16. bxc5 Bxc5 17. Qc4 Bd6 18. Nxd4 Nc5 19. Nxc6 bxc6 20. Bxc6+ Bd7 21. Bxa8 Kf8 22. Bf3 Be6 23. Qb5 Bd7 24. Qb8+ Qe8 25. Qa7 Rg8 26. g4 h5 27. h3 hxg4 28. hxg4 Bxg4 29. Bg2 Bh3 30. Rfe1 Rxg2+ 31. Kh1 Ne4 32. Qd4 f3 33. Qh8+ Ke7 34. Qxh3 Kf8 35. Qxf3 Nxf2+ 36. Kxg2 1-0");
    } */

    /// <summary>
    /// Looks through entire chessboard to find a king of given color.
    /// </summary>
    /// <param name="kingColor">Color of king.</param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="attackingColor"></param>
    /// <returns>True if square is attacked by any piece of given color.</returns>
    public bool IsSquareAttacked (Vector2Int position, PieceColor attackingColor)
    {
        foreach (Piece piece in PiecesOnBoard)
        {
            if (piece == null)
                continue;

            if (piece.Color != attackingColor)
                continue;

            foreach (Vector2Int attackedPosition in piece.SquareAttacker.AttackedSquares)
            {
                if (attackedPosition == position)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Recalculates everything.
    /// </summary>
    public void OnChessStateUpdate ()
    {
        King whiteKing = GetKing(PieceColor.WHITE);
        King blackKing = GetKing(PieceColor.BLACK);

        whiteKing.CheckingPieces.Clear();
        blackKing.CheckingPieces.Clear();

        for (int i = 0; i <= 2; i++)
        {
            foreach (Piece piece in PiecesOnBoard)
            {
                if (piece == null)
                    continue;

                switch (i)
                {
                    case 0:
                        piece.UnpinPiece();
                        break;
                    case 1:
                        piece.CalculateAttackedSquares();
                        break;
                    case 2:
                        piece.CalculateLegalMoves();
                        break;
                }
            }
        }

        UCI.CalculatePosition(FEN.ConvertChessboardToFEN());
        OpeningsBook.GetOpeningName(PGN.GetMovesHistory(false));
    }

    private void PrepareBoard ()
    {
        FEN.ConvertFenToChessboard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        OnChessStateUpdate();
    }

    public void ClearBoard ()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                PiecesOnBoard[x, y] = null;
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
    /// Executes move.
    /// </summary>
    public bool ExecuteMove (Move move)
    {
        if (move.From.x < 0 || move.From.x > boardSize - 1 || move.From.y < 0 || move.From.y > boardSize - 1)
            return false;

        if (move.To.x < 0 || move.To.x > boardSize - 1 || move.To.y < 0 || move.To.y > boardSize - 1)
            return false;

        Piece pieceToMove = PiecesOnBoard[move.From.x, move.From.y];

        if (pieceToMove == null)
            return false;

        if (pieceToMove.Color != ColorToMove)
            return false;

        if (!pieceToMove.IsMoveLegal(move.To))
            return false;

        if (PiecesOnBoard[move.To.x, move.To.y] != null)
        {
            MovesWithoutCaptureCount = 0;
        }
        else
        {
            MovesWithoutCaptureCount++;
        }

        string algebraicNotationMove = PGN.ConvertMoveToAlgebraicNotation(move.From, move.To, move.Promotion);
        PGN.AddAlgebraicNotation(algebraicNotationMove);

        PiecesOnBoard[move.To.x, move.To.y] = pieceToMove;
        PiecesOnBoard[move.From.x, move.From.y] = null;
        pieceToMove.SetPosition(move.To);

        ColorToMove = ColorToMove == PieceColor.WHITE ? PieceColor.BLACK : PieceColor.WHITE;
        TotalMovesCount++;
        OnPieceMoved?.Invoke(this, move);
        OnChessStateUpdate();
        return true;
    }

    /// <summary>
    /// Executes move.
    /// </summary>
    /// <param name="from">From tile position.</param>
    /// <param name="to">To tile position.</param>
    /// <param name="promotion">If pawn is about to get promoted (q,b,n,r), otherwise white space.</param>
    /// <returns>True if move has been successfuly executed & false on illegal moves.</returns>
    public bool ExecuteMove (Vector2Int from, Vector2Int to, char promotion = ' ')
    {
        return ExecuteMove(new Move(from, to, promotion));
    }
}
