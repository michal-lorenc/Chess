using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    public Vector2Int Position { get; private set; }
    public PieceColor Color { get; private set; }
    public PieceType Type { get; protected set; }
    public SquareAttacker SquareAttacker { get; private set; }

    protected List<Vector2Int> legalMovesPositions = new List<Vector2Int>();
    
    /// <summary>
    /// Returns true if this piece has limited movement due to protecting King from attacks.
    /// </summary>
    public bool IsPinned { get { return pinnedByPiece != null; } }
    protected Piece pinnedByPiece = null;

    public Piece (PieceColor pieceColor, Vector2Int position)
    {
        SquareAttacker = new SquareAttacker(this);
        Position = position;
        Color = pieceColor;
    }

    public void PinPiece (Piece pinnedByPiece)
    {
        this.pinnedByPiece = pinnedByPiece; 
    }

    public void UnpinPiece ()
    {
        pinnedByPiece = null;
    }

    public void SetPosition (Vector2Int position)
    {
        Position = position;
    }

    public bool IsMoveLegal (Vector2Int moveToTest)
    {
        foreach (Vector2Int legalMove in GetLegalMoves())
        {
            if (legalMove == moveToTest)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns previously calculated legal moves.
    /// </summary>
    /// <returns>Vector2Int list of legal positions it can move to.</returns>
    public virtual List<Vector2Int> GetLegalMoves ()
    {
        return legalMovesPositions;
    }

    /// <summary>
    /// Checks what moves are legal for this piece.
    /// This method should stay the same for all piece types except 'King'.
    /// </summary>
    /// <returns>Vector2Int list of legal positions it can move to.</returns>
    public virtual List<Vector2Int> CalculateLegalMoves ()
    {
        King ourKing = Chess.Singleton.GetKing(Color);

        // No moves are possible when king is mated.
        if (ourKing.IsMated)
        {
            legalMovesPositions.Clear(); 
            return null;
        }

        // No move can safe our king if he is double checked.
        if (ourKing.IsDoubleChecked)
        {
            legalMovesPositions.Clear();
            return null;
        }

        // Try to find move for this piece that would save it from check
        if (ourKing.IsChecked)
        {
            // If this piece is pinned while it's king is checked it means it can't save it
            if (IsPinned)
            {
                legalMovesPositions.Clear();
                return null;
            }

            List<Vector2Int> kingSavingMoves = new List<Vector2Int>();

            // Find moves that will save king from being checked.
            foreach (Vector2Int possibleMove in GetMoves())
            {
                foreach (Vector2Int threateningMove in ourKing.CheckingPieces[0].SquareAttacker.PotentiallyAttackedSquares)
                {
                    if (possibleMove == threateningMove)
                    {
                        kingSavingMoves.Add(possibleMove);
                        break;
                    }
                }
            }

            legalMovesPositions = kingSavingMoves;
            return legalMovesPositions;
        }

        if (IsPinned)
        {
            List<Vector2Int> possibleMoves = new List<Vector2Int>();

            foreach (Vector2Int possibleMove in GetMoves())
            {
                foreach (Vector2Int threateningMove in pinnedByPiece.SquareAttacker.PotentiallyAttackedSquares)
                {
                    if (possibleMove == threateningMove)
                    {
                        possibleMoves.Add(possibleMove);
                        break;
                    }
                }
            }

            Debug.Log("Im pinned and my moveset is reduced :(");

            legalMovesPositions = possibleMoves;
            return legalMovesPositions;
        }
        else // If this piece is not pinned, it means that GetMoves() moves are valid.
        {
            legalMovesPositions = GetMoves();
            return legalMovesPositions;
        }
    }

    /// <summary>
    /// 'Calculate Attacked Squares' should be called before this method.
    /// Returns attacked squares, except for those on which the piece of our color stands.
    /// Moves returned by this method might not be legal.
    /// </summary>
    /// <returns>Vector2Int list of theoretically possible moves.</returns>
    protected virtual List<Vector2Int> GetMoves ()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        foreach (Vector2Int squarePosition in SquareAttacker.AttackedSquares)
        {
            if (Chess.Singleton.PiecesOnBoard[squarePosition.x, squarePosition.y] != null && 
                Chess.Singleton.PiecesOnBoard[squarePosition.x, squarePosition.y].Color == Color)
            {
                continue;
            }

            possibleMoves.Add(squarePosition);
        }

        return possibleMoves;
    }

    /// <summary>
    /// Calculates which square are attacked by this piece.
    /// It also calculates potentially attacked squares, so we know which pieces are pinned.
    /// </summary>
    public virtual void CalculateAttackedSquares ()
    {
        SquareAttacker.ClearAttackedSquares();

        // Always add itself as potentially attacked square so we know how to exactly save king
        SquareAttacker.PotentiallyAttackedSquares.Add(Position);
    }
}
