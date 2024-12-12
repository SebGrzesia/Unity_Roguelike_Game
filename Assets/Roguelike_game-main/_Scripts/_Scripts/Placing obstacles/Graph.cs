using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private static List<Vector2Int> neighbourse4directions = new List<Vector2Int>()
    {
        new Vector2Int(0,1), //Up
        new Vector2Int(1,0), //Right
        new Vector2Int(0,-1), //Down
        new Vector2Int(-1,0) //Left
    };

    private static List<Vector2Int> neighbourse8Directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //Right
        new Vector2Int(0,-1), //Down
        new Vector2Int(-1,0), //left
        new Vector2Int(1,1), //Diagonal
        new Vector2Int(1,-1), //Diagonal
        new Vector2Int(-1,1), //Diagonal
        new Vector2Int(-1,-1) //Diagonal
    };

    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices)
    {
        graph = new List<Vector2Int>(vertices);
    }

    public List<Vector2Int> GetNeighbouts4Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse4directions);
    }

    public List<Vector2Int> GetNeighbouts8Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse8Directions);

    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neihboursOffsetList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach (var neighboiurDirection in neihboursOffsetList)
        {
            Vector2Int potentialNeibours = startPosition + neighboiurDirection;
            if (graph.Contains(potentialNeibours))
            {
                neighbours.Add(potentialNeibours);
            }
        }
        return neighbours;
    }
}
