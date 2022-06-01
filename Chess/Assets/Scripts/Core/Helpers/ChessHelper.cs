using System;
using UnityEngine;

public static class ChessHelper
{
    public static PieceType CharToPieceType (char pieceChar)
    {
        pieceChar = char.ToUpper(pieceChar);

        switch (pieceChar)
        {
            case 'K':
                return PieceType.KING;
            case 'Q':
                return PieceType.QUEEN;
            case 'B':
                return PieceType.BISHOP;
            case 'R':
                return PieceType.ROOK;
            case 'N':
                return PieceType.KNIGHT;
            case 'P':
                return PieceType.PAWN;
            default:
                throw new ArgumentException();
        }
    }

    public static char PieceTypeToChar (PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.KING:
                return 'K';
            case PieceType.QUEEN:
                return 'Q';
            case PieceType.BISHOP:
                return 'B';
            case PieceType.ROOK:
                return 'R';
            case PieceType.KNIGHT:
                return 'N';
            case PieceType.PAWN:
                return 'P';
            default:
                throw new ArgumentException();
        }
    }

    public static int CharToNumber (char posChar)
    {
        posChar = char.ToUpper(posChar);

        switch (posChar)
        {
            case 'A':
                return 0;
            case 'B':
                return 1;
            case 'C':
                return 2;
            case 'D':
                return 3;
            case 'E':
                return 4;
            case 'F':
                return 5;
            case 'G':
                return 6;
            case 'H':
                return 7;
            default:
                throw new ArgumentException();
        }
    }

    public static char NumberToChar (int number)
    {
        switch (number)
        {
            case 0:
                return 'a';
            case 1:
                return 'b';
            case 2:
                return 'c';
            case 3:
                return 'd';
            case 4:
                return 'e';
            case 5:
                return 'f';
            case 6:
                return 'g';
            case 7:
                return 'h';
            default:
                throw new ArgumentException();
        }
    }

    public static string Vector2IntPositionToStringPosition (Vector2Int position)
    {
        return NumberToChar(position.x).ToString() + (position.y + 1);
    }

    public static Vector2Int StringPositionToVector2IntPosition (string position)
    {
        int x = CharToNumber(position[0]);
        int y = Convert.ToInt32(position[1].ToString()) - 1;
        return new Vector2Int(x, y);
    }

    public static char PieceColorToChar (PieceColor color)
    {
        char pieceColorChar = color == PieceColor.WHITE ? 'w' : 'b';
        return pieceColorChar;
    }

    public static PieceColor CharToPieceColor (char color)
    {
        PieceColor pieceColor = color == 'w' ? PieceColor.WHITE : PieceColor.BLACK;
        return pieceColor;
    }
}
