using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChessboardUI : MonoBehaviour
{
    [Header("Design"), SerializeField] 
    private Color32 primaryColor; 
    [SerializeField] 
    private Color32 secondaryColor;
    [SerializeField, Space(3)]
    private PieceSprites whitePieceSprites;
    [SerializeField]
    private PieceSprites blackPieceSprites;
    [SerializeField, Space(3)]
    private GameObject piecePrefab;
    [SerializeField]
    private GameObject dotPrefab;

    private Transform[,] squareTransforms = new Transform[Chess.boardSize, Chess.boardSize];
    private GameObject[,] dots = new GameObject[Chess.boardSize, Chess.boardSize];
    private MeshFilter meshFilter;

    private void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        RenderChessboard();
        InstantiatePieces();

        ShowLegalMoves(new Vector2Int(1, 0));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);
        }

    }

    public void RenderChessboard ()
    {
        List<Vector3> vertex = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color32> colors = new List<Color32>();

        for (int x = 0; x < Chess.boardSize; x++)
        {
            for (int y = 0; y < Chess.boardSize; y++)
            {
                int vertexBefore = vertex.Count;
                vertex.AddRange(new List<Vector3> { new Vector3(x, y), new Vector3(x, y + 1), new Vector3(x + 1, y + 1), new Vector3(x + 1, y) });
                triangles.AddRange(new List<int> { 0 + vertexBefore, 1 + vertexBefore, 2 + vertexBefore, 0 + vertexBefore, 2 + vertexBefore, 3 + vertexBefore});

                if ((y + x) % 2 == 0)
                    colors.AddRange(new List<Color32>() { primaryColor, primaryColor, primaryColor, primaryColor });
                else
                    colors.AddRange(new List<Color32>() { secondaryColor, secondaryColor, secondaryColor, secondaryColor });

                CreateSquareTransform(x, y);
            }
        }

        meshFilter.sharedMesh.SetVertices(vertex);
        meshFilter.sharedMesh.SetTriangles(triangles, 0);
        meshFilter.sharedMesh.SetColors(colors);
    }

    private void CreateSquareTransform (int x, int y)
    {
        Transform squareTransform = new GameObject().transform;
        squareTransform.name = $"SQUARE ({x},{y})";
        squareTransform.SetParent(transform);
        squareTransform.localPosition = new Vector3(x + 0.5f, y + 0.5f, -0.001f);
        squareTransforms[x, y] = squareTransform;

        CreateDot(squareTransform, x, y);
    }

    private void CreateDot (Transform parentSquare, int x, int y)
    {
        GameObject dot = Instantiate(dotPrefab, parentSquare);
        dot.transform.localPosition = new Vector3(0, 0, 0);
        dots[x, y] = dot;
        dot.SetActive(false);
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

                Instantiate(piecePrefab).GetComponent<PieceUI>().SetPiece(piece.Color == PieceColor.WHITE ? whitePieceSprites.GetSprite(piece.Type) : blackPieceSprites.GetSprite(piece.Type), squareTransforms[x, y]);
            }
        }
    }

    /// <summary>
    /// Displays 
    /// </summary>
    /// <param name="position"></param>
    private void ShowLegalMoves (Vector2Int position)
    {
        HideLegalMoves();

        Piece piece = Chess.Singleton.PiecesOnBoard[position.x, position.y];

        if (piece == null)
            return;

        foreach (Vector2Int legalMove in piece.GetLegalMoves())
        {
            dots[legalMove.x, legalMove.y].SetActive(true);
        }
    }

    private void HideLegalMoves ()
    {
        foreach (GameObject dot in dots)
            dot.SetActive(false);
    }
}
