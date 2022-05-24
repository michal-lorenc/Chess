using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that let's you calculate attacked and potentially attacked square positions.
/// </summary>
public class SquareAttacker
{
    /// <summary>
    /// List of Vector2Int position of attacked squares.
    /// It also contains defended pieces positions, so enemy king knows what pieces he cannot take.
    /// </summary>
    public List<Vector2Int> AttackedSquares { get; private set; } = new List<Vector2Int>();

    /// <summary>
    /// Potentially attacked squares are those that pin enemy piece so that it cannot move because it has to defend king.
    /// If we pin an opposing piece, the only possible moves for that opposite piece are squares that can be attacked,
    /// as long as the opposing piece has the ability to do so in its possible moves.
    /// </summary>
    public List<Vector2Int> PotentiallyAttackedSquares { get; private set; } = new List<Vector2Int>();

    // Temporary private variables that are needed for loops to work properly.
    private List<Vector2Int> potentiallyAttackedSquaresTemporary = new List<Vector2Int>();
    private bool potentiallyAttackedSquaresTemporaryIsValid = false;
    private Piece potentiallyPinnedPiece = null;

    /// <summary>
    /// Reference to piece that instantiated this object.
    /// </summary>
    private readonly Piece parentPiece;

    public SquareAttacker (Piece parentPiece)
    {
        this.parentPiece = parentPiece;
    }

    /// <summary>
    /// Attacks all diagonal squares, just like bishop.
    /// </summary>
    public void AttackDiagonalSquares ()
    {
        // Diagonal up right
        for (int x = parentPiece.Position.x + 1; x < Chess.boardSize; x++)
        {
            int y = parentPiece.Position.y + (x - parentPiece.Position.x);

            if (y > Chess.boardSize - 1)
                break;

            if (!CheckSquare(x, y))
                break;
        }

        MergeAndClear();

        // Diagonal down right
        for (int x = parentPiece.Position.x + 1; x < Chess.boardSize; x++)
        {
            int y = parentPiece.Position.y - (x - parentPiece.Position.x);

            if (y < 0)
                break;

            if (!CheckSquare(x, y))
                break;
        }

        MergeAndClear();

        // Diagonal up left
        for (int x = parentPiece.Position.x - 1; x >= 0; x--)
        {
            int y = parentPiece.Position.y + (parentPiece.Position.x - x);

            if (y > Chess.boardSize - 1)
                break;

            if (!CheckSquare(x, y))
                break;
        }

        MergeAndClear();

        // Diagonal down left
        for (int x = parentPiece.Position.x - 1; x >= 0; x--)
        {
            int y = parentPiece.Position.y - (parentPiece.Position.x - x);

            if (y < 0)
                break;

            if (!CheckSquare(x, y))
                break;
        }

        MergeAndClear();
    }

    /// <summary>
    /// Attacks horizontal squares (x axis).
    /// </summary>
    public void AttackHorizontalSquares ()
    {
        // Horizontal right side movement
        for (int x = parentPiece.Position.x + 1; x < Chess.boardSize; x++)
        {
            if (!CheckSquare(x, parentPiece.Position.y))
                break;
        }

        MergeAndClear();

        // Horizontal left side movement
        for (int x = parentPiece.Position.x - 1; x >= 0; x--)
        {
            if (!CheckSquare(x, parentPiece.Position.y))
                break;
        }

        MergeAndClear();
    }

    /// <summary>
    /// Attacks vertical squares (y axis).
    /// </summary>
    public void AttackVerticalSquares ()
    {
        // Vertical up side movement
        for (int y = parentPiece.Position.y + 1; y < Chess.boardSize; y++)
        {
            if (!CheckSquare(parentPiece.Position.x, y))
                break;
        }

        MergeAndClear();

        // Vertical down side movement
        for (int y = parentPiece.Position.y - 1; y >= 0; y--)
        {
            if (!CheckSquare(parentPiece.Position.x, y))
                break;
        }

        MergeAndClear();
    }

    /// <summary>
    /// Attacks all neighbour squares, just like king do.
    /// </summary>
    public void AttackNeighbourSquares ()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                if (parentPiece.Position.x + x < 0 || parentPiece.Position.x + x > Chess.boardSize - 1)
                    continue;

                if (parentPiece.Position.y + y < 0 || parentPiece.Position.y + y > Chess.boardSize - 1)
                    continue;

                AttackedSquares.Add(new Vector2Int(parentPiece.Position.x + x, parentPiece.Position.y + y));
            }
        }
    }


    public void AttackSquaresPawnStyle ()
    {
        Vector2Int[] positions = new Vector2Int[]
        {
            new Vector2Int(parentPiece.Position.x + 1, parentPiece.Color == PieceColor.WHITE ? parentPiece.Position.y + 1 : parentPiece.Position.y - 1),
            new Vector2Int(parentPiece.Position.x - 1, parentPiece.Color == PieceColor.WHITE ? parentPiece.Position.y + 1 : parentPiece.Position.y - 1)
        };

        King enemyKing = Chess.Singleton.GetKing(parentPiece.Color == PieceColor.WHITE ? PieceColor.BLACK : PieceColor.WHITE);

        foreach (Vector2Int position in positions)
        {
            if (position.x < 0 || position.x > Chess.boardSize - 1)
                continue;

            if (position.y < 0 || position.y > Chess.boardSize - 1)
                continue;

            if (enemyKing.Position == position)
                enemyKing.CheckingPieces.Add(parentPiece);

            AttackedSquares.Add(position);
        }
    }

    public void AttackSquaresKnightStyle ()
    {
        Vector2Int[] positions = new Vector2Int[] 
        { 
            new Vector2Int(parentPiece.Position.x + 1, parentPiece.Position.y + 2),
            new Vector2Int(parentPiece.Position.x - 1, parentPiece.Position.y + 2),
            new Vector2Int(parentPiece.Position.x + 1, parentPiece.Position.y - 2),
            new Vector2Int(parentPiece.Position.x - 1, parentPiece.Position.y - 2),
            new Vector2Int(parentPiece.Position.x - 2, parentPiece.Position.y + 1),
            new Vector2Int(parentPiece.Position.x - 2, parentPiece.Position.y - 1),
            new Vector2Int(parentPiece.Position.x + 2, parentPiece.Position.y + 1),
            new Vector2Int(parentPiece.Position.x + 2, parentPiece.Position.y - 1),
        };

        King enemyKing = Chess.Singleton.GetKing(parentPiece.Color == PieceColor.WHITE ? PieceColor.BLACK : PieceColor.WHITE);

        foreach (Vector2Int position in positions)
        {
            if (position.x < 0 || position.x > Chess.boardSize - 1)
                continue;

            if (position.y < 0 || position.y > Chess.boardSize - 1)
                continue;

            if (enemyKing.Position == position)
                enemyKing.CheckingPieces.Add(parentPiece);

            AttackedSquares.Add(position);
        }
    }

    /// <summary>
    /// Clears all data about attacked squares.
    /// </summary>
    public void ClearAttackedSquares ()
    {
        AttackedSquares.Clear();
        PotentiallyAttackedSquares.Clear();
    }

    /// <summary>
    /// Do not call this method outside 'CalculateAttackedSquares' method.
    /// </summary>
    /// <param name="x">X position on board</param>
    /// <param name="y">Y position on board</param>
    /// <returns>Returns true if the loop that executed this method should be continued and false if should be breaked.</returns>
    private bool CheckSquare (int x, int y)
    {
        Piece piece = Chess.Singleton.PiecesOnBoard[x, y];

        // if the square is empty
        if (piece == null)
        {
            if (potentiallyPinnedPiece == null)
            {
                AttackedSquares.Add(new Vector2Int(x, y));
                PotentiallyAttackedSquares.Add(new Vector2Int(x, y));
            }
            else
            {
                potentiallyAttackedSquaresTemporary.Add(new Vector2Int(x, y));
            }

            return true;
        }

        if (piece.Color == parentPiece.Color)
        {
            if (potentiallyPinnedPiece == null)
                AttackedSquares.Add(new Vector2Int(x, y));

            return false;
        }
        else
        {
            if (piece.Type == PieceType.KING)
            {
                if (potentiallyPinnedPiece == null)
                {
                    AttackedSquares.Add(new Vector2Int(x, y));
                    potentiallyAttackedSquaresTemporary.Add(new Vector2Int(x, y));

                    King enemyKing = piece as King;
                    enemyKing.CheckingPieces.Add(parentPiece);
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
                    AttackedSquares.Add(new Vector2Int(x, y));
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
    private void MergeAndClear ()
    {
        if (potentiallyAttackedSquaresTemporaryIsValid)
        {
            PotentiallyAttackedSquares.AddRange(potentiallyAttackedSquaresTemporary);
            potentiallyPinnedPiece.PinPiece(parentPiece);
        }

        potentiallyAttackedSquaresTemporary.Clear();
        potentiallyPinnedPiece = null;
        potentiallyAttackedSquaresTemporaryIsValid = false;
    }
}
