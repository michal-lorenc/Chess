using UnityEngine;

[System.Serializable]
public struct Move
{
    [field: SerializeField] public Vector2Int From { get; private set; }
    [field: SerializeField] public Vector2Int To { get; private set; }
    [field: SerializeField] public char Promotion { get; private set; }

    public Move (string longAlgebraicNotation)
    {
        From = ChessHelper.StringPositionToVector2IntPosition(longAlgebraicNotation[0].ToString() + longAlgebraicNotation[1].ToString());
        To = ChessHelper.StringPositionToVector2IntPosition(longAlgebraicNotation[2].ToString() + longAlgebraicNotation[3].ToString());

        if (longAlgebraicNotation.Length > 4)
            Promotion = longAlgebraicNotation[4];
        else
            Promotion = ' ';
    }

    public Move (Vector2Int from, Vector2Int to, char promotion)
    {
        From = from;
        To = to;
        Promotion = promotion;
    }
}
