using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PieceFactory
{
    public static Piece CreatePieceFromFEN (char fenSymbol, Vector2Int position)
    {
        PieceColor color = char.IsUpper(fenSymbol) ? PieceColor.WHITE : PieceColor.BLACK;

        fenSymbol = char.ToUpper(fenSymbol);

        switch (fenSymbol)
        {
            case 'K':
                return new King(color, position);
            case 'Q':
                return new Queen(color, position);
            case 'R':
                return new Rook(color, position);
            case 'B':
                return new Bishop(color, position);
            case 'N':
                return new Knight(color, position);
            case 'P':
                return new Pawn(color, position);
            default:
                throw new ArgumentException();
        }
    }
}
