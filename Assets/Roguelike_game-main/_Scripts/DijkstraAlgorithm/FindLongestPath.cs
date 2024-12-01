using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLongestPath : MonoBehaviour
{
    public static (Vector2Int, Vector2Int) FindTwoFurthestRooms(List<Vector2Int> roomCenters)
    {
        Vector2Int startRoom = roomCenters[0];
        var distanceFromStart = Dijkstra(roomCenters, startRoom);
        Vector2Int farthestRoom = GetFarthestRoom(distanceFromStart);

        var distancesFromFarthest = Dijkstra(roomCenters, farthestRoom);
        Vector2Int otherFarthestRoom = GetFarthestRoom(distancesFromFarthest);

        return (farthestRoom, otherFarthestRoom);
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
