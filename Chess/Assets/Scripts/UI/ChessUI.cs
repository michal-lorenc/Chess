using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(ContentSizeFitter))]
public class ChessUI : MonoBehaviour
{
    [field: SerializeField] public ChessSkin Skin { get; private set; }
    public TileUI[,] tiles = new TileUI[Chess.boardSize, Chess.boardSize];
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject piecePrefab;

    [HideInInspector] public PieceUI draggedPiece = null;
    [HideInInspector] public PieceUI selectedPiece = null;
    [Header("References"), SerializeField] private CoordinatesUI coordinatesUI;
    [SerializeField] private EvaluationBarUI evaluationBarUI;
    [SerializeField] private MoveMarkerUI moveMarkerUI;
    private GridLayoutGroup gridLayoutGroup;

    public Chess Chess { get; private set; }
    public static ChessUI Singleton { get; private set; }

    private void Awake ()
    {
        Singleton = this;
        Chess = new Chess();
        Chess.OnPieceMoved += (s, e) => OnPieceMoved(e);
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start ()
    {
        CreateBoard();
        InstantiatePieces();
        HideLegalMoves();
        RotateBoard(PieceColor.WHITE);
    }

    private void OnApplicationPause (bool pause)
    {
        if (pause)
        {
            if (Chess != null)
                Chess.UCI.StopChessEngine();
        }
    }

    private void OnApplicationQuit ()
    {
        if (Chess != null)
            Chess.UCI.StopChessEngine();
    }

    public void OnPieceMoved (Move move)
    {
        if (!Skin.HighlightMoves)
            return;

        foreach (TileUI tile in tiles)
            tile.SetHighlight(false);

        tiles[move.From.x, move.From.y].SetHighlight(true);
        tiles[move.To.x, move.To.y].SetHighlight(true);
    }

    public void RotateBoard ()
    {
        if (gridLayoutGroup.startCorner == GridLayoutGroup.Corner.LowerLeft)
            RotateBoard(PieceColor.BLACK);
        else
            RotateBoard(PieceColor.WHITE);
    }

    public void RotateBoard (PieceColor color)
    {
        if (color == PieceColor.WHITE)
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
        else
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperRight;

        coordinatesUI.SwitchDisplay(color);
        evaluationBarUI.SwitchDisplay(color);
        StartCoroutine(moveMarkerUI.OnBoardRotatedIE());
    }

    private void CreateBoard ()
    {
        for (int y = 0; y < Chess.boardSize; y++)
        {
            for (int x = 0; x < Chess.boardSize; x++)
            {
                tiles[x, y] = Instantiate(tilePrefab, transform).GetComponent<TileUI>();
                tiles[x, y].SetColor((y + x) % 2 == 0 ? Skin.PrimaryColor : Skin.SecondaryColor);
                tiles[x, y].SetHighlightColor(Skin.HighlightColor);
                tiles[x, y].position = new Vector2Int(x, y);
            }
        }
    }

    private void CreatePiece ()
    {

    }

    private void InstantiatePieces ()
    {
        for (int x = 0; x < Chess.boardSize; x++)
        {
            for (int y = 0; y < Chess.boardSize; y++)
            {
                Piece piece = Chess.PiecesOnBoard[x, y];

                if (piece == null)
                    continue;

                Sprite pieceSprite = piece.Color == PieceColor.WHITE ? Skin.WhitePieceSprites.GetSprite(piece.Type) : Skin.BlackPieceSprites.GetSprite(piece.Type);
                Instantiate(piecePrefab).GetComponent<PieceUI>().SetPiece(pieceSprite).SetParent(tiles[x, y]);
            }
        }
    }


    public void ShowLegalMoves (Vector2Int position)
    {
        HideLegalMoves();

        if (!Skin.DisplayLegalMoves)
            return;

        Piece piece = Chess.PiecesOnBoard[position.x, position.y];

        if (piece == null)
            return;

        foreach (Vector2Int legalMove in piece.GetLegalMoves())
        {
            tiles[legalMove.x, legalMove.y].ShowLegalMoveMark();
        }
    }

    public void HideLegalMoves ()
    {
        foreach (var tile in tiles)
            tile.HideLegalMoveMark();
    }
}
