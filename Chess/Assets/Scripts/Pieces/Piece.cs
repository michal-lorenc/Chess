using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    protected Vector2Int position;
    public PieceColor Color { get; private set; }
    public PieceType Type { get; protected set; }
    
    /// <summary>
    /// Returns true if this piece has limited movement due to protecting King from attacks.
    /// </summary>
    public bool IsPinned { get { return pinnedByPiece != null; } }
    protected Piece pinnedByPiece = null;

    public void PinPiece (Piece pinnedByPiece)
    {
        this.pinnedByPiece = pinnedByPiece; 
    }

    public void UnpinPiece ()
    {
        pinnedByPiece = null;
    }


    /// <summary>
    /// Checks what moves are legal for this piece.
    /// This method should stay the same for all piece types except 'King'.
    /// </summary>
    /// <returns>Vector2Int list of legal positions it can move to.</returns>
    public virtual List<Vector2Int> GetLegalMoves ()
    {
        King ourKing = Chess.Singleton.GetKing(Color);

        // No moves are possible when king is mated.
        if (ourKing.IsMated)
            return null;

        // No move can safe our king if he is double checked.
        if (ourKing.IsDoubleChecked)
            return null;

        // Try to find move for this piece that would save it from check
        if (ourKing.IsChecked)
        {
            // If this piece is pinned while it's king is checked it means it can't save it
            if (IsPinned)
                return null;

            List<Vector2Int> kingSavingMoves = new List<Vector2Int>();

            // Find moves that will save king from being checked.
            foreach (Vector2Int possibleMove in GetMoves())
            {
                foreach (Vector2Int threateningMove in ourKing.CheckingPieces[0].potentiallyAttackedSquares)
                {
                    if (possibleMove == threateningMove)
                    {
                        kingSavingMoves.Add(possibleMove);
                        break;
                    }
                }
            }

            return kingSavingMoves;
        }

        if (IsPinned)
        {
            List<Vector2Int> possibleMoves = new List<Vector2Int>();

            foreach (Vector2Int possibleMove in GetMoves())
            {
                foreach (Vector2Int threateningMove in pinnedByPiece.potentiallyAttackedSquares)
                {
                    if (possibleMove == threateningMove)
                    {
                        possibleMoves.Add(possibleMove);
                        break;
                    }
                }
            }

            return possibleMoves;
        }
        else // If this piece is not pinned, it means that GetMoves() moves are valid.
        {
            return GetMoves();
        }
    }

    /// <summary>
    /// Moves returned by this method might not be legal.
    /// </summary>
    /// <returns>Vector2Int list of theoretically possible moves.</returns>
    protected abstract List<Vector2Int> GetMoves();


    public Piece (PieceColor pieceColor, Vector2Int position)
    {
        this.position = position;
        Color = pieceColor;
    }

    public virtual void CalculateAttackedSquares ()
    {
        attackedSquares.Clear();
        potentiallyAttackedSquares.Clear();
    }

    public List<Vector2Int> attackedSquares = new List<Vector2Int>();
    public List<Vector2Int> potentiallyAttackedSquares = new List<Vector2Int>();
    private List<Vector2Int> potentiallyAttackedSquaresTemporary = new List<Vector2Int>();
    private bool potentiallyAttackedSquaresTemporaryIsValid = false;
    private Piece potentiallyPinnedPiece = null;

    /// <summary>
    /// Do not call this method outside 'CalculateAttackedSquares' method.
    /// </summary>
    /// <param name="x">X position on board</param>
    /// <param name="y">Y position on board</param>
    /// <returns>Returns true if the loop that executed this method should be continued and false if should be breaked.</returns>
    protected bool CheckSquare (int x, int y)
    {
        Piece piece = Chess.Singleton.PiecesOnBoard[x, y];

        // if the square is empty
        if (piece == null)
        {
            if (potentiallyPinnedPiece == null)
            {
                attackedSquares.Add(new Vector2Int(x, y));
                potentiallyAttackedSquares.Add(new Vector2Int(x, y));
            }
            else
            {
                potentiallyAttackedSquaresTemporary.Add(new Vector2Int(x, y));
            }

            return true;
        }

        if (piece.Color == Color)
        {
            if (potentiallyPinnedPiece == null)
                attackedSquares.Add(new Vector2Int(x, y));

            return false;
        }
        else
        {
            if (piece.Type == PieceType.KING)
            {
                if (potentiallyPinnedPiece == null)
                {
                    attackedSquares.Add(new Vector2Int(x, y));
                    potentiallyAttackedSquaresTemporary.Add(new Vector2Int(x, y));

                    King enemyKing = piece as King;
                    enemyKing.CheckingPieces.Add(this);
                }
                else
                {
                    potentiallyAttackedSquaresTemporary.Add(new Vector2Int(x, y));
                }

                potentiallyAttackedSquaresTemporaryIsValid = true;

                return false;
            }
            else
            {
                if (potentiallyPinnedPiece == null)
                {
                    potentiallyPinnedPiece = piece;
                    attackedSquares.Add(new Vector2Int(x, y));
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    /// <summary>
    /// Should be called after loop.
    /// Resets temporary variables to default state.
    /// Decides if it wants to merge temporary data with final data.
    /// </summary>
    protected void MergeAndClear ()
    {
        if (potentiallyAttackedSquaresTemporaryIsValid)
        {
            potentiallyAttackedSquares.AddRange(potentiallyAttackedSquaresTemporary);
            potentiallyPinnedPiece.PinPiece(this);
        }

        potentiallyAttackedSquaresTemporary.Clear();
        potentiallyPinnedPiece = null;
        potentiallyAttackedSquaresTemporaryIsValid = false;
    }

}
