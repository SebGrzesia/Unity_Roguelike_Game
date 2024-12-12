using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected RandomWalkSO randomWalkParams;


    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParams, startPossition);

        Dictionary<Vector2Int, TitleProperties> tileProperties = new Dictionary<Vector2Int, TitleProperties>();
        foreach (var position in floorPositions)
        {
            if (!tileProperties.ContainsKey(position))
            {
                tileProperties[position] = new TitleProperties(position, passable: true);
            }
        }

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer, tileProperties);
    }

    protected HashSet<Vector2Int> RunRandomWalk(RandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iteration; i++)
        {
            var path = ProceduralGerationAlgorithms.RandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
