using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public static MapData Instance { get; private set; }

    public List<HashSet<Vector2Int>> Rooms { get; private set; } = new List<HashSet<Vector2Int>>();
    public HashSet<Vector2Int> Corridors { get; private set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> Walls { get; private set; } = new HashSet<Vector2Int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ClearData()
    {
        Rooms.Clear();
        Corridors.Clear();
        Walls.Clear();
    }

    public void AddRoom(HashSet<Vector2Int> roomFloor)
    {
        Rooms.Add(roomFloor);
    }

    public void AddCorridors(HashSet<Vector2Int> corridorFloor)
    {
        Corridors.UnionWith(corridorFloor);
    }

    public void AddWalls(HashSet<Vector2Int> wallPosition)
    {
        Walls.UnionWith(wallPosition);
    }
}
