using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI openingNameText;
    [SerializeField] private TextMeshProUGUI movesHistoryText;

    private void Start ()
    {
        ChessUI.Singleton.Chess.OpeningsBook.OnOpeningRecognized += (s, e) => { openingNameText.text = e; };
        ChessUI.Singleton.Chess.PGN.OnMoveHistoryUpdate += (s, e) => { movesHistoryText.text = e; };
    }

}
