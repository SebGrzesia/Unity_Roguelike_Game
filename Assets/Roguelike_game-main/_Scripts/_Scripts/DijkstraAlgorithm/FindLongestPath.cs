using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLongestPath : MonoBehaviour
{
    public static (Vector2Int, Vector2Int, Vector2Int) FindThreeFurthestRooms(List<Vector2Int> roomCenters)
    {
        // Find start point
        Vector2Int startRoom = roomCenters[0];

        // find farthest point from start point
        var distanceFromStart = Dijkstra(roomCenters, startRoom);
        Vector2Int firstFarthestRoom = GetFarthestRoom(distanceFromStart);

        // find farthest point from previous point
        var distancesFromFirst = Dijkstra(roomCenters, firstFarthestRoom);
        Vector2Int secondFarthestRoom = GetFarthestRoom(distancesFromFirst);

        // Find the third farthest point
        Vector2Int thirdFarthestRoom = GetThirdFarthestRoom(roomCenters, firstFarthestRoom, secondFarthestRoom);

        return (firstFarthestRoom, secondFarthestRoom, thirdFarthestRoom);
    }

    private static Dictionary<Vector2Int, float> Dijkstra(List<Vector2Int> roomCenters, Vector2Int start)
    {
        var distances = new Dictionary<Vector2Int, float>();
        var priorityQueue = new SortedSet<(float distance, Vector2Int position)>(new DistanceComparer());

        foreach (var room in roomCenters)
        {
            distances[room] = float.MaxValue;
        }
        distances[start] = 0;
        priorityQueue.Add((0, start));

        while (priorityQueue.Count > 0)
        {
            var (currentDistance, currentRoom) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            foreach (var neighbor in roomCenters)
            {
                if (neighbor == currentRoom) continue;

                float distanceToNeighbor = Vector2.Distance(currentRoom, neighbor);
                float newDistance = currentDistance + distanceToNeighbor;

                if (newDistance < distances[neighbor])
                {
                    priorityQueue.Remove((distances[neighbor], neighbor));

                    distances[neighbor] = newDistance;
                    priorityQueue.Add((newDistance, neighbor));
                }
            }
        }
        return distances;
    }

    private static Vector2Int GetFarthestRoom(Dictionary<Vector2Int, float> distances)
    {
        Vector2Int farthestRoom = Vector2Int.zero;
        float maxDistance = float.MinValue;

        foreach (var room in distances)
        {
            if (room.Value > maxDistance)
            {
                maxDistance = room.Value;
                farthestRoom = room.Key;
            }
        }
        return farthestRoom;
    }

    private static Vector2Int GetThirdFarthestRoom(List<Vector2Int> roomCenters, Vector2Int room1, Vector2Int room2)
    {
        Vector2Int thirdFarthestRoom = Vector2Int.zero;
        float maxCombinedDistance = float.MinValue;

        foreach (var room in roomCenters)
        {
            if (room == room1 || room == room2) continue;

            float distanceToRoom1 = Vector2.Distance(room, room1);
            float distanceToRoom2 = Vector2.Distance(room, room2);

            float combinedDistance = distanceToRoom1 + distanceToRoom2;

            if (combinedDistance > maxCombinedDistance)
            {
                maxCombinedDistance = combinedDistance;
                thirdFarthestRoom = room;
            }
        }

        return thirdFarthestRoom;
    }
}

public class DistanceComparer : IComparer<(float distance, Vector2Int position)>
{
    public int Compare((float distance, Vector2Int position) x, (float distance, Vector2Int position) y)
    {
        int distanceComparison = x.distance.CompareTo(y.distance);

        if (distanceComparison != 0)
            return distanceComparison;

        return x.position.GetHashCode().CompareTo(y.position.GetHashCode());
    }
}
