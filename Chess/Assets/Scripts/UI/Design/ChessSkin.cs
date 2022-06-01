using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skins/New Skin", order = 1)]
public class ChessSkin : ScriptableObject
{
    [field: SerializeField] public Color32 PrimaryColor { get; private set; }
    [field: SerializeField] public Color32 SecondaryColor { get; private set; }
    [field: SerializeField] public Color32 HighlightColor { get; private set; }

    [field: SerializeField, Space(5)] public PieceSprites WhitePieceSprites { get; private set; }
    [field: SerializeField] public PieceSprites BlackPieceSprites { get; private set; }

    [field: SerializeField, Space(5)] public bool DisplayLegalMoves { get; private set; }
    [field: SerializeField] public bool HighlightMoves { get; private set; }
    [field: SerializeField] public bool SoundsEnabled { get; private set; }
}
