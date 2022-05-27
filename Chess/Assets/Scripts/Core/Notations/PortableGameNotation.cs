using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PortableGameNotation 
{
    [SerializeField] private List<string> whiteMoves = new List<string>();
    [SerializeField] private List<string> blackMoves = new List<string>();

    public PortableGameNotation (List<string> whiteMoves, List<string> blackMoves)
    {
        this.whiteMoves = whiteMoves;
        this.blackMoves = blackMoves;
    }

    public PortableGameNotation ()
    {

    }


    public static PortableGameNotation CreateFromString (string pgnString)
    {
        string[] pgnSplittedData = pgnString.Split(']');

        string[] moves = pgnSplittedData[pgnSplittedData.Length - 1].Split(' ');

        bool isWhiteMove = true;
        List<string> whiteMoves = new List<string>();
        List<string> blackMoves = new List<string>();

        foreach (string move in moves)
        {
            if (string.IsNullOrWhiteSpace(move))
                continue;

            if (move.Contains("."))
                continue;

            if (isWhiteMove)
                whiteMoves.Add(move);
            else
                blackMoves.Add(move);

            isWhiteMove = !isWhiteMove;
        }

        return new PortableGameNotation(whiteMoves, blackMoves);
    }
}
