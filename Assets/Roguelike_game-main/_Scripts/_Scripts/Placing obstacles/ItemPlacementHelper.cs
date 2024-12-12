using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPlacementHelper
{
    Dictionary<PlacementType, HashSet<Vector2Int>>
        tileByType = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    //HashSet<Vector2Int> roomFloorNoCorridor;
    HashSet<Vector2Int> roomFloorNoCorridorAndNoWall;

    public ItemPlacementHelper()
    {
        tileByType.Clear();

        List<Vector2> allFloorsExceptCorridors = GetAllRooms();
        List<Vector2> allCorridors = GetAllCorridor();
        this.roomFloorNoCorridorAndNoWall = new HashSet<Vector2Int>(allFloorsExceptCorridors.Select(pos => Vector2Int.FloorToInt(pos)));

        Graph graphNoWalls = new Graph(this.roomFloorNoCorridorAndNoWall);
        PlacementType type;

        foreach (var position in this.roomFloorNoCorridorAndNoWall)
        {
            int neighboursCount8Dir = graphNoWalls.GetNeighbouts8Directions(position).Count;
            int neighboursCount4Dir = graphNoWalls.GetNeighbouts4Directions(position).Count;

            if (neighboursCount8Dir == 8)
            {
                type = PlacementType.OpenSpace;
            }
            else if ( neighboursCount4Dir < 4)
            {
                type = PlacementType.NearWall;
            }
            else
            {
                continue;
            }

            if (!tileByType.ContainsKey(type))
            {
                tileByType[type] = new HashSet<Vector2Int>();
            }

            tileByType[type].Add(position);
        }

        foreach (var kvp in tileByType)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value.Count} positions");
        }
    }


    //public ItemPlacementHelper()
    //{
    //    tileByType.Clear();

    //    List<Vector2> allFloorsExceptCorridors = GetAllFloorsExceptCorridors();

    //    this.roomFloorNoCorridorAndNoWall = new HashSet<Vector2Int>(allFloorsExceptCorridors.Select(pos => Vector2Int.FloorToInt(pos)));

    //    //this.roomFloorNoCorridor = MapData.Instance.Rooms.SelectMany(room => room).Except(MapData.Instance.Corridors).ToHashSet();
    //    //this.roomFloorNoCorridorAndNoWall = MapData.Instance.Rooms.SelectMany(room => room).Except(MapData.Instance.Corridors).Except(MapData.Instance.Walls).ToHashSet();

    //    Graph graphNoWalls = new Graph(this.roomFloorNoCorridorAndNoWall);
    //    PlacementType type;

    //    foreach (var position in this.roomFloorNoCorridorAndNoWall)
    //    {
    //        int neighboursCount8Dir = graphNoWalls.GetNeighbouts8Directions(position).Count;
    //        int neighboursCount4Dir = graphNoWalls.GetNeighbouts4Directions(position).Count;

    //        if (neighboursCount8Dir == 8)
    //        {
    //            type = PlacementType.OpenSpace;
    //        }
    //        else if (neighboursCount4Dir <= 4)
    //        {
    //            type = PlacementType.NearWall;
    //        }
    //        //PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;
    //        //PlacementType type = neightb == 0 ? PlacementType.OpenSpace : PlacementType.NearWall;

    //        if (tileByType.ContainsKey(type) == false)
    //        {
    //            tileByType[type] = new HashSet<Vector2Int>();
    //        }

    //        //if (type == PlacementType.NearWall && graphNoWalls.GetNeighbouts4Directions(position).Count == 2) // zeby nie bylo w rogach?
    //        //    continue;
    //        tileByType[type].Add(position);
    //    }
    //    foreach (var kvp in tileByType)
    //    {
    //        Debug.Log($"{kvp.Key}: {kvp.Value.Count} positions");

    //    }
    //}

    //public Vector2? GetItemPlacementPositin(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    //{ 
    //    int itemArea = size.x * size.y;
    //    if (tileByType[placementType].Count < itemArea)
    //        return null;

    //    int iteration = 0;
    //    while (iteration < iterationsMax)
    //    {
    //        iteration++;
    //        Debug.Log($"Iteration: {iteration}");

    //        int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
    //        Vector2Int position = tileByType[placementType].ElementAt(index);

    //        Debug.Log($"Trying position: {position}");

    //        if (itemArea > 1)
    //        {
    //            var (result, placementPositions) = PlaceBigItem(position, size, addOffset);

    //            if (result == false)
    //            {
    //                Debug.Log("Failed to place big item.");
    //                continue;
    //            }

    //            tileByType[placementType].ExceptWith(placementPositions);
    //            tileByType[PlacementType.NearWall].ExceptWith(placementPositions);
    //        }
    //        else
    //        {
    //            tileByType[placementType].Remove(position);
    //        }

    //        Debug.Log($"Placement successful at: {position}");
    //        return position;
    //    }
    //    return null;
    //}

    private (bool, List<Vector2Int>) PlaceBigItem(
        Vector2Int originPosition,
        Vector2Int size,
        bool addOffset
        )
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if (col == 0 && row == 0)
                {
                    continue;
                }
                Vector2Int newPosToCheck =
                    new Vector2Int(originPosition.x + row, originPosition.y + col);
                if (this.roomFloorNoCorridorAndNoWall.Contains(newPosToCheck) == false)
                    return (false, positions);
                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    }

    public List<Vector2> GetAllFloors()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var item in MapData.Instance.MyFloors)
        {
            pos.Add(item);
        }
        return pos;
    }

    public List<Vector2> GetAllCorridor()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var item in MapData.Instance.Corridors)
        {
            pos.Add(item);
        }
        return pos;
    }

    public List<Vector2> GetAllFloorsExceptCorridors()
    {
        HashSet<Vector2Int> allFloors = MapData.Instance.MyFloors;
        HashSet<Vector2Int> allCorridors = MapData.Instance.Corridors;
        IEnumerable<Vector2Int> floorsExceptCorridors = allFloors.Except(allCorridors);
        List<Vector2> pos = floorsExceptCorridors.Select(tile => (Vector2)tile).ToList();

        return pos;
    }




    public List<Vector2> GetAllWalls()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var item in MapData.Instance.Walls)
        {
            pos.Add(item);
        }
        return pos;
    }

    public List<Vector2> GetAllRooms()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var room in MapData.Instance.Rooms) 
        {
            foreach (var tile in room)
            {
                pos.Add(tile);
            }
        }
        return pos;
    }

    public HashSet<Vector2Int> GetTilesByType(PlacementType type)
    {
        if (tileByType.ContainsKey(type))
        {
            return tileByType[type];
        }
        return new HashSet<Vector2Int>();
    }


    public List<Vector2> GetAllRoomsNoCorridorNoWalls()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var item in MapData.Instance.Rooms.SelectMany(room => room).Except(MapData.Instance.Corridors).Except(MapData.Instance.Walls))
        {
            pos.Add(item);
        }
        return pos;
    }
}


public enum PlacementType
{
    OpenSpace,
    NearWall
}