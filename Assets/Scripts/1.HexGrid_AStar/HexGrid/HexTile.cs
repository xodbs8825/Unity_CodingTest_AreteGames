using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexTileStatus { VALID, INVALID }

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexTile : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private List<Face> faces;

    public Material material;
    public float innerSize;
    public float outerSize = 1;
    public bool isFlatTopped;
    public bool isPathValid;
    public HexTileStatus TILE_STATUS;

    public Vector2Int offsetCoordinate;
    public Vector3Int cubeCoordinate;
    public List<HexTile> neighbours;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "Hex";

        meshFilter.mesh = mesh;
        meshRenderer.material = material;

        isPathValid = true;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    private void OnDrawGizmosSelected()
    {
        foreach (HexTile neighbour in neighbours)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, neighbour.transform.position);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        }
    }

    private void OnMouseDown()
    {
        isPathValid = !isPathValid;
        TILE_STATUS = isPathValid ? HexTileStatus.VALID : HexTileStatus.INVALID;
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        faces = new List<Face>();

        for (int point = 0; point < 6; point++)
        {
            faces.Add(CreateFace(innerSize, outerSize, point));
        }
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < faces.Count; i++)
        {
            vertices.AddRange(faces[i].vertices);
            uvs.AddRange(faces[i].uvs);

            int offset = (4 * i);
            foreach (int triangle in faces[i].triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private Face CreateFace(float innerRadius, float outerRadius, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRadius, point);
        Vector3 pointB = GetPoint(innerRadius, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRadius, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRadius, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    private Vector3 GetPoint(float size, int index)
    {
        float angle_deg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angle_rad = Mathf.PI / 180 * angle_deg;
        return new Vector3(size * Mathf.Cos(angle_rad), 0, size * Mathf.Sin(angle_rad));
    }

    public void OnSelectTile()
    {
        GridManager.instance.OnSelectTile(this);
    }
}