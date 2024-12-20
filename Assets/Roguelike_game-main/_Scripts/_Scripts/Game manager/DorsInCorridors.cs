using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorsInCorridors : MonoBehaviour
{
    public GameObject doorPrefab;
    public Transform doorsParent;

    public void SpawnDoorsInCorridor()
    {
        var corridors = new HashSet<Vector2Int>(MapData.Instance.Corridors);
        var rooms = new HashSet<Vector2Int>();

        foreach ( var room in MapData.Instance.Rooms )
        {
            rooms.UnionWith( room );
        }

        corridors.ExceptWith( rooms );

        Graph corridorGraph = new Graph(corridors);

        foreach (var corridorTile in corridors)
        {
            int neighbourCount = corridorGraph.GetNeighbouts4Directions(corridorTile).Count;

            //if ( neighbourCount == 3 )
            //{
                Instantiate(doorPrefab, new Vector3(corridorTile.x + 0.5f, corridorTile.y + 0.5f, 0), Quaternion.identity, doorsParent);
            //}
        }
    }
}
