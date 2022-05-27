using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(ContentSizeFitter))]
public class ChessUI : MonoBehaviour
{
    [Header("Design"), SerializeField]
    private Color32 primaryColor;
    [SerializeField]
    private Color32 secondaryColor;
    [SerializeField, Space(3)]
    private PieceSprites whitePieceSprites;
    [SerializeField]
    private PieceSprites blackPieceSprites;

    private TileUI[,] tiles = new TileUI[Chess.boardSize, Chess.boardSize];
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject piecePrefab;

    public static ChessUI Singleton { get; private set; }

    private void Awake ()
    {
        Singleton = this;
    }

    private void Start ()
    {
        CreateBoard();
        InstantiatePieces();
        HideLegalMoves();
    }

    private void CreateBoard ()
    {
        for (int y = 0; y < Chess.boardSize; y++)
        {
            for (int x = 0; x < Chess.boardSize; x++)
            {
                tiles[x, y] = Instantiate(tilePrefab, transform).GetComponent<TileUI>();
                tiles[x, y].SetColor((y + x) % 2 == 0 ? primaryColor : secondaryColor);
                tiles[x, y].position = new Vector2Int(x, y);
            }
        }
    }

    private void CreatePiece ()
    {

    }

    private void InstantiatePieces()
    {
        for (int x = 0; x < Chess.boardSize; x++)
        {
            for (int y = 0; y < Chess.boardSize; y++)
            {
                Piece piece = Chess.Singleton.PiecesOnBoard[x, y];

                if (piece == null)
                    continue;

                Instantiate(piecePrefab).GetComponent<PieceUI>().SetPiece(piece.Color == PieceColor.WHITE ? whitePieceSprites.GetSprite(piece.Type) : blackPieceSprites.GetSprite(piece.Type), tiles[x, y].transform);
            }
        }
    }

    public void RotateBoard ()
    {

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
