using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(ContentSizeFitter))]
public class ChessUI : MonoBehaviour
{
    [field: SerializeField] public ChessSkin Skin { get; private set; }
    private TileUI[,] tiles = new TileUI[Chess.boardSize, Chess.boardSize];
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject piecePrefab;

    public PieceUI draggedPiece = null;
    public PieceUI selectedPiece = null;
    [Header("References"), SerializeField]
    private CoordinatesUI coordinatesUI;
    [SerializeField] private EvaluationBarUI evaluationBarUI;
    private GridLayoutGroup gridLayoutGroup;

    public static ChessUI Singleton { get; private set; }

    private void Awake ()
    {
        Singleton = this;
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start ()
    {
        CreateBoard();
        InstantiatePieces();
        HideLegalMoves();
        RotateBoard(PieceColor.WHITE);
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
    }

    private void CreateBoard ()
    {
        for (int y = 0; y < Chess.boardSize; y++)
        {
            for (int x = 0; x < Chess.boardSize; x++)
            {
                tiles[x, y] = Instantiate(tilePrefab, transform).GetComponent<TileUI>();
                tiles[x, y].SetColor((y + x) % 2 == 0 ? Skin.PrimaryColor : Skin.SecondaryColor);
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
                Piece piece = Chess.Singleton.PiecesOnBoard[x, y];

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

        Piece piece = Chess.Singleton.PiecesOnBoard[position.x, position.y];

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
