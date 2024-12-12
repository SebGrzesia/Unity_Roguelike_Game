using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private List<TileBase> floorTiles;

    [SerializeField]
    private TileBase wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, Vector2Int position)
    {
        // Check random tile
        TileBase randomTile = floorTiles[UnityEngine.Random.Range(0, floorTiles.Count)];

        // Convert position on Vector3Int
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);

        // Set tile
        tilemap.SetTile(tilePosition, randomTile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        foreach (Transform child in wallTilemap.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Tilemapy i WallCollidery zostały wyczyszczone.");
    }

    internal void PaintSignleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        // Check type of wall and set up proper tile
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        // set up tile and collider
        if (tile != null)
        {
            PaintWallTile(wallTilemap, tile, position);
            AddBoxColliderToWall(position);
        }
    }

    private void PaintWallTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        tilemap.SetTile(tilePosition, tile);
    }

    private void AddBoxColliderToWall(Vector2Int position)
    {
        // convert tile possition on world position
        Vector3 worldPosition = wallTilemap.CellToWorld(new Vector3Int(position.x, position.y, 0));

        // Create new object to wall collision
        GameObject wallCollider = new GameObject($"WallCollider_{position.x}_{position.y}");
        wallCollider.transform.position = worldPosition;
        wallCollider.transform.SetParent(wallTilemap.transform);

        // Add boxCollider component
        BoxCollider2D collider = wallCollider.AddComponent<BoxCollider2D>();

        // Set up collizion size
        collider.size = new Vector2(1, 1);
        collider.offset = new Vector2(0.5f, 0.5f);
        collider.isTrigger = false;
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        // Set up proper tile name
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        // Set tile on tilemap
        if (tile != null)
        {
            PaintWallTile(wallTilemap, tile, position);
        }
    }
}
