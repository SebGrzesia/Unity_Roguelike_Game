using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public static MapData Instance { get; private set; }

    public List<HashSet<Vector2Int>> Rooms { get; private set; } = new List<HashSet<Vector2Int>>();
    public HashSet<Vector2Int> Corridors { get; private set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> Walls { get; private set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> MyFloors { get; set; } = new HashSet<Vector2Int>();

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

    public void ClearData()
    {
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.DestroyAllChildOfThisObject();
        }
        Rooms.Clear();
        Corridors.Clear();
        Walls.Clear();
        MyFloors.Clear();
        Debug.Log("MapData CLeared");
    }

    public void ClearAllEnemies()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    //public void AddRoom(HashSet<Vector2Int> room)
    //{
    //    if (room == null || room.Count == 0)
    //    {
    //        Debug.LogWarning("Tried to add an empty or null room.");
    //        return;
    //    }

    //    Rooms.Add(room);
    //    Debug.Log($"Added room with {room.Count} tiles. Total rooms: {Rooms.Count}");
    //}
}
