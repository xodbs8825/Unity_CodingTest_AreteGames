using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HexTile currentTile;

    private HexTile nextTile;
    private List<HexTile> currentPath;
    private Vector3 targetPos;
    private bool gotPath;

    public LineRenderer renderer;

    private void Update()
    {
        currentPath = GridManager.instance.path;
        transform.position = currentTile.transform.position;
    }

    protected void UpdateLineRenderer(List<HexTile> tiles)
    {
        if (renderer == null) { return; }

        List<Vector3> points = new List<Vector3>();
        foreach (HexTile tile in tiles)
        {
            points.Add(tile.transform.position + new Vector3(0, .5f, 0));
        }

        renderer.positionCount = points.Count;
        renderer.SetPositions(points.ToArray());
    }

    public void MovePlayer()
    {
        if (currentPath == null || currentPath.Count <= 1)
        {
            nextTile = null;

            if (currentPath != null && currentPath.Count > 0)
            {
                currentTile = currentPath[0];
                nextTile = currentTile;
            }

            gotPath = false;
            UpdateLineRenderer(new List<HexTile>());
        }
        else
        {
            currentTile = currentPath[0];
            nextTile = currentPath[1];

            if (nextTile.TILE_STATUS != HexTileStatus.VALID)
            {
                currentPath.Clear();
                MovePlayer();
                return;
            }

            targetPos = nextTile.transform.position + new Vector3(0, 1f, 0);
            gotPath = true;
            currentPath.RemoveAt(0);
            GridManager.instance.playerPos = nextTile.cubeCoordinate;
            UpdateLineRenderer(currentPath);
        }
    }
}
