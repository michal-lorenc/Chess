using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameResult
{
    public Winner Winner { get; private set; }
    public Reason Reason { get; private set; }

    public GameResult (Winner winner, Reason reason)
    {
        Winner = winner;
        Reason = reason;
    }

}