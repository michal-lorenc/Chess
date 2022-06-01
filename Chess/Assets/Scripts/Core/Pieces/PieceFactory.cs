using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PieceFactory
{
    public static Piece CreatePieceFromFEN (char fenSymbol, Vector2Int position, Chess chess)
    {
        PieceColor color = char.IsUpper(fenSymbol) ? PieceColor.WHITE : PieceColor.BLACK;

        fenSymbol = char.ToUpper(fenSymbol);

        switch (fenSymbol)
        {
            case 'K':
                return new King(color, position, chess);
            case 'Q':
                return new Queen(color, position, chess);
            case 'R':
                return new Rook(color, position, chess);
            case 'B':
                return new Bishop(color, position, chess);
            case 'N':
                return new Knight(color, position, chess);
            case 'P':
                return new Pawn(color, position, chess);
            default:
                throw new ArgumentException();
        }
    }
}
