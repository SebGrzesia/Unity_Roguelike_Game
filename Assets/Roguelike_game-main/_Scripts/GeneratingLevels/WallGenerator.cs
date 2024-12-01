using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer, Dictionary<Vector2Int, TitleProperties> tileProperties)
    {
        var basicWallPositions = FindWallsInDirection(floorPosition, Direction2D.cardinalDirectionsList);
        var corrnerWallPositions = FindWallsInDirection(floorPosition, Direction2D.diagonalDirectionsList);

        foreach (var position in basicWallPositions)
        {
            if (!tileProperties.ContainsKey(position))
            {
                tileProperties[position] = new TitleProperties(position, passable: false);
            }
        }

        foreach (var position in corrnerWallPositions)
        {
            if (!tileProperties.ContainsKey(position))
            {
                tileProperties[position] = new TitleProperties(position, passable: false);
            }
        }

        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPosition);
        CreateCornerWalls(tilemapVisualizer, corrnerWallPositions, floorPosition);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> corrnerWallPositions, HashSet<Vector2Int> floorPosition)
    {
        foreach (var position in corrnerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPosition.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPosition)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPosition.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSignleBasicWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPosition.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
