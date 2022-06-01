using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMarkerUI : MonoBehaviour
{
    [SerializeField] private LineRendererUI lineRendererUI;
    private Move currentMove;

    private void Start ()
    {
       ChessUI.Singleton.Chess.UCI.OnGoodMoveFound += (s, e) => OnGoodMoveFound(e);
       ChessUI.Singleton.Chess.UCI.OnBestMoveFound += (s, e) => OnGoodMoveFound(e);
    }

    private void OnGoodMoveFound (Move move)
    {
        currentMove = move;
        Vector3 fromPosition = ChessUI.Singleton.tiles[move.From.x, move.From.y].transform.localPosition;
        Vector3 toPosition = ChessUI.Singleton.tiles[move.To.x, move.To.y].transform.localPosition;

        lineRendererUI.SetPoints(new List<Vector2> { fromPosition, toPosition });
    }

    public void ToggleDisplay ()
    {
        lineRendererUI.enabled = !lineRendererUI.enabled;
    }

    public IEnumerator OnBoardRotatedIE ()
    {
        yield return null;
        OnGoodMoveFound(currentMove);
    }
}
