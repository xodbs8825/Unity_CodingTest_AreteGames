using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Size")]
    public Vector2Int gridSize;

    [Header("Materials")]
    public Material validMaterial;
    public Material invalidMaterial;

    [Header("Offsets")]
    public float innerSize;
    public float outerSize = 1f;
    public float radius = 1f;
    public bool isFlatTopped;

    private Dictionary<Vector3Int, HexTile> tiles;
    public List<HexTile> path;
    private bool isDirty = false;

    public static GridManager instance = null;

    public Player player;
    public Vector3Int playerPos;

    private void Start()
    {
        StartCoroutine(Tick());
    }

    private void Update()
    {
        if (isDirty)
        {
            LayOutGrid();
            isDirty = false;
        }

        UpdateHexRenderer();
    }

    private void Awake()
    {
        instance = this;
        isDirty = false;
        LayOutGrid();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            isDirty = true;
        }
    }

    public void Clear()
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            children.Add(child);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child, true);
        }
    }

    private void LayOutGrid()
    {
        if (Application.isPlaying)
        {
            Clear();
        }

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x}, {y}", typeof(HexTile));
                tile.transform.position = Utilities.GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                tile.AddComponent<MeshCollider>();

                HexTile hexTile = tile.GetComponent<HexTile>();
                hexTile.innerSize = innerSize;
                hexTile.outerSize = outerSize;
                hexTile.isFlatTopped = isFlatTopped;
                hexTile.SetMaterial(validMaterial);
                hexTile.DrawMesh();

                hexTile.offsetCoordinate = new Vector2Int(x, y);
                hexTile.cubeCoordinate = Utilities.OffsetToCube(hexTile.offsetCoordinate);

                tile.transform.SetParent(transform, true);

                SetCubeCoordinateText(hexTile.cubeCoordinate, tile);
            }
        }

        tiles = new Dictionary<Vector3Int, HexTile>();

        HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
        foreach (HexTile hexTile in hexTiles)
        {
            RegisterTile(hexTile);
        }
        foreach (HexTile hexTile in hexTiles)
        {
            List<HexTile> neighbours = GetNeighbours(hexTile);
            hexTile.neighbours = neighbours;
        }

        HexTile playerTile = tiles[Vector3Int.zero];
        playerPos = playerTile.cubeCoordinate;
        player.transform.position = playerTile.transform.position + new Vector3(0, 1f, 0);
        player.currentTile = playerTile;
    }
    
    private void SetCubeCoordinateText(Vector3Int cubeCoordinate, GameObject parent)
    {
        GameObject coordinate = new GameObject("CubeCoordinate", typeof(TextMesh));
        coordinate.transform.SetParent(parent.transform, true);

        coordinate.transform.Rotate(90.0f, .0f, .0f);
        coordinate.transform.localScale = Vector3.one * .5f;

        TextMesh text = coordinate.GetComponent<TextMesh>();

        text.text = $"[{cubeCoordinate.x}, {cubeCoordinate.y}, {cubeCoordinate.z}]";

        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.characterSize = .5f;
        text.color = Color.black;
        coordinate.transform.localPosition = Vector3.zero;

        coordinate.SetActive(true);
    }

    private void UpdateHexRenderer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            HexTile hex = transform.GetChild(i).GetComponent<HexTile>();

            Material material = hex.isPathValid ? validMaterial : invalidMaterial;
            hex.SetMaterial(material);
            
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                GameObject text = hex.transform.GetChild(0).gameObject;
                text.SetActive(!text.activeSelf);
            }

        }
    }

    private List<HexTile> GetNeighbours(HexTile tile)
    {
        List<HexTile> neighbours = new List<HexTile>();

        Vector3Int[] neighbourCoords = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(0, -1, 1)
        };

        foreach (Vector3Int neighbourCoord in neighbourCoords)
        {
            Vector3Int tileCoord = tile.cubeCoordinate;

            if (tiles.TryGetValue(tileCoord + neighbourCoord, out HexTile neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public void RegisterTile(HexTile tile)
    {
        tiles.Add(tile.cubeCoordinate, tile);
    }

    public void OnSelectTile(HexTile tile)
    {
        path = PathFinding.FindPath(player.currentTile, tile);
    }

    public IEnumerator Tick()
    {
        player.MovePlayer();
        yield return new WaitForSeconds(.5f);
        StartCoroutine(Tick());
    }
}
