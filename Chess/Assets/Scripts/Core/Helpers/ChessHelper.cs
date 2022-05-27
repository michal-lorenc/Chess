using System;

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
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            case 4:
                return 'E';
            case 5:
                return 'F';
            case 6:
                return 'G';
            case 7:
                return 'H';
            default:
                throw new ArgumentException();
        }
    }
}
