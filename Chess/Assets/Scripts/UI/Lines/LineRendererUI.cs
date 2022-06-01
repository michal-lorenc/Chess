using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LineRendererUI : Graphic
{
    [SerializeField] private List<Vector2> points = new List<Vector2>();
    [SerializeField] private float width = 2;
    [SerializeField] private float arrowSize = 2;

    public void SetPoints (List<Vector2> points)
    {
        this.points = points;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh (VertexHelper vh)
    {
        if (points.Count < 2)
        {
            vh.Clear();
            return;
        }

        List<Vector2> vertex = new List<Vector2>();
        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;
        Vector2 normalizedVector;

        for (int i = 0; i < points.Count; i++)
        {
            int trianglesCountBefore = vertex.Count;

            if (i + 1 >= points.Count)
                normalizedVector = (points[i] - points[i - 1]).normalized;
            else
                normalizedVector = (points[i] - points[i + 1]).normalized;

            vertex.Add(new Vector3(points[i].x - (normalizedVector.y * width), points[i].y + (normalizedVector.x * width), 0));
            vertex.Add(new Vector3(points[i].x + (normalizedVector.y * width), points[i].y - (normalizedVector.x * width), 0));

            vert.position = new Vector2(points[i].x - (normalizedVector.y * width), points[i].y + (normalizedVector.x * width));
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(points[i].x + (normalizedVector.y * width), points[i].y - (normalizedVector.x * width));
            vert.color = color;
            vh.AddVert(vert);

            if (i != points.Count - 1)
            {
                if (i == points.Count - 2)
                {
                    vh.AddTriangle(0 + trianglesCountBefore, 2 + trianglesCountBefore, 3 + trianglesCountBefore);
                    vh.AddTriangle(2 + trianglesCountBefore, 0 + trianglesCountBefore, 1 + trianglesCountBefore);
                }
                else
                {
                    vh.AddTriangle(0 + trianglesCountBefore, 2 + trianglesCountBefore, 3 + trianglesCountBefore);
                    vh.AddTriangle(3 + trianglesCountBefore, 0 + trianglesCountBefore, 1 + trianglesCountBefore);
                }
            }
        }

        // draw arrow
        normalizedVector = (points[points.Count - 1] - points[points.Count - 2]).normalized;

        vert.position = new Vector2(points[points.Count - 1].x - (normalizedVector.y * (width + arrowSize)), points[points.Count - 1].y + (normalizedVector.x * (width + arrowSize)));
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(points[points.Count - 1].x + (normalizedVector.y * (width + arrowSize)), points[points.Count - 1].y - (normalizedVector.x * (width + arrowSize)));
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(points[points.Count - 1].x + (normalizedVector.x * (width + arrowSize * 2.5f) / 2), points[points.Count - 1].y + (normalizedVector.y * (width + arrowSize * 2.5f) / 2));
        vert.color = color;
        vh.AddVert(vert);

        vh.AddTriangle(0 + vertex.Count, 1 + vertex.Count, 2 + vertex.Count);
    }

}
    

