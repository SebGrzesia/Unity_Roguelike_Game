using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : RandomWalkDungeonGenerator
{
    public PlayerController Player;

    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    public GameObject entrance, exit, key;

    private List<BoundsInt> roomsList;

    public void GenerateNewLevel()
    {
        RunProceduralGeneration();
    }

    protected override void RunProceduralGeneration()
    {
        var tileProperties = new Dictionary<Vector2Int, TitleProperties>();
        CreateRoom(tileProperties);

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        var (roomA, roomB, roomC) = FindLongestPath.FindThreeFurthestRooms(roomCenters);

        Debug.Log($"Najdalsze pokoje to: {roomA}, {roomB}, i {roomC}");
        BoardManager board = FindObjectOfType<BoardManager>();

        board.SetSpawnTile(roomA);

        if (board != null)
        {
            board.InitializeTiles(tileProperties);
        }

        GenerateEntranceAndExitDor(roomA, roomB, roomC);

        GenerateItems();
    }


    private void GenerateEntranceAndExitDor(Vector2Int roomA, Vector2Int roomB, Vector2Int roomC)
    {
        GenerateSpriteOnPosition(roomA, entrance);
        GenerateSpriteOnPosition(roomB, exit);
        GenerateSpriteOnPosition(roomC, key);
    }

    private void GenerateSpriteOnPosition(Vector2Int position, GameObject sprite)
    {
        //Change tile pozition to world position
        Vector3 worldPosition = BoardManager.Instance.CellToWorld(position);

        GameObject spawnedSprite = Instantiate(sprite, worldPosition, Quaternion.identity);
        
    }

    private void GenerateItems()
    {
        Debug.Log("Generating items");
        var placementHelper = new ItemPlacementHelper();

        ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
        if ( itemSpawner != null )
        {
            itemSpawner.SpawnObstacle1x1( placementHelper );
            itemSpawner.SpawnBiggerObstacleInOpenSpace(placementHelper);
        }
        else
        {
            Debug.LogError("ItemSpawner not found in the scene");
        }
    }

    //private void CreateRoom(Dictionary<Vector2Int, TitleProperties> tileProperties)
    //{
    //    //clear singleton Map data
    //    MapData.Instance.ClearData();

    //    roomsList = ProceduralGerationAlgorithms.BinarySpacePartitioning(
    //        new BoundsInt((Vector3Int)startPossition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),
    //        minRoomWidth,
    //        minRoomHeight
    //    );

    //    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

    //    if (randomWalkRooms)
    //    {
    //        floor = CreateRoomsRandomly(roomsList);
    //    }
    //    else
    //    {
    //        floor = CreateSimpleRooms(roomsList);
    //    }

    //    //Add rooms to singleton Map
    //    foreach (var room in roomsList)
    //    {
    //        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
    //        for (int col = offset; col < room.size.x - offset; col++)
    //        {
    //            for (int row = offset; row < room.size.y; row++)
    //            {
    //                Vector2Int position = (Vector2Int)room.min + new Vector2Int(row, col);
    //                roomFloor.Add(position);
    //            }
    //        }
    //        MapData.Instance.AddRoom(roomFloor);
    //    } 

    //    List<Vector2Int> roomCenters = new List<Vector2Int>();
    //    foreach (var room in roomsList)
    //    {
    //        roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
    //    }

    //    HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

    //    //Add coridor to singleton Map
    //    MapData.Instance.AddCorridors(corridors);

    //    floor.UnionWith(corridors);

    //    foreach (var position in floor)
    //    {
    //        if (!tileProperties.ContainsKey(position))
    //        {
    //            tileProperties[position] = new TitleProperties(position, passable: true);
    //        }
    //    }

    //    tilemapVisualizer.PaintFloorTiles(floor);
    //    var wallPositions = WallGenerator.CreateWalls(floor, tilemapVisualizer, tileProperties);
    //    MapData.Instance.MyFloors = floor;
    //    MapData.Instance.AddWalls(wallPositions);
    //}

    private void CreateRoom(Dictionary<Vector2Int, TitleProperties> tileProperties)
    {
        //clear singleton Map data
        BoardManager.Instance.ClearBoardData();
        MapData.Instance.ClearData();

        roomsList = ProceduralGerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPossition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),
            minRoomWidth,
            minRoomHeight
        );

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

        // Add corridor to singleton Map
        MapData.Instance.AddCorridors(corridors);

        floor.UnionWith(corridors);

        foreach (var position in floor)
        {
            if (!tileProperties.ContainsKey(position))
            {
                tileProperties[position] = new TitleProperties(position, passable: true);
            }
        }

        tilemapVisualizer.PaintFloorTiles(floor);

        //Creating wall
        var wallPositions = WallGenerator.CreateWalls(floor, tilemapVisualizer, tileProperties);
        MapData.Instance.MyFloors = floor;
        MapData.Instance.AddWalls(wallPositions);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var roomBounds in roomsList)
        {
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParams, roomCenter);
            HashSet<Vector2Int> roomTiles = new HashSet<Vector2Int>();

            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset)
                    && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                    roomTiles.Add(position);
                }
            }

            // Add Generated room to MapData
            MapData.Instance.AddRoom(roomTiles);
        }
        return floor;
    }


    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindColsestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindColsestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
