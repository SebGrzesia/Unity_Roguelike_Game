using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawner : MonoBehaviour
{
    public List<SpawnableItem> itemsToSpawn1x1;
    public List<SpawnableItem> BigerObstacleToSpawn;
    public GameObject itemsParent;

    public void SpawnObstacle1x1(ItemPlacementHelper placementHelper)
    {
        Debug.Log("Spawning obstacle 1x1");


        if (itemsToSpawn1x1 == null || itemsToSpawn1x1.Count == 0)
        {
            Debug.LogWarning("No obstacle newar wall to spawn!");
            return;
        }

        var nearWallPostion = placementHelper.GetTilesByType(PlacementType.NearWall).ToList();

        var corridorPositions = MapData.Instance.Corridors;
        nearWallPostion.RemoveAll(pos => corridorPositions.Contains(pos));

        if ( nearWallPostion.Count <= 0 )
        {
            Debug.LogWarning("No position to spawn");
            return;
        }

        foreach ( var obstacle in itemsToSpawn1x1)
        {
            int quantityToSpawn = Random.Range(obstacle.minQuantityPerRoom, obstacle.maxQuantityPerRoom);

            for ( int i = 0; i < quantityToSpawn; i++ )
            {
                int randomIndex = Random.Range(0,nearWallPostion.Count);
                Vector2Int spawnPosition = nearWallPostion[randomIndex];

                var worldPosition = BoardManager.Instance.CellToWorld(spawnPosition);

                GameObject spawnedObstacle = Instantiate(obstacle.prefab, worldPosition, Quaternion.identity, itemsParent.transform);

                if (!obstacle.isWalkable)
                {
                    BoxCollider2D collider = spawnedObstacle.AddComponent<BoxCollider2D>();
                    collider.isTrigger = false;
                }

                nearWallPostion.RemoveAt(randomIndex);
                placementHelper.occupiedTiles.Add(spawnPosition);

                if (nearWallPostion.Count == 0) break;
            }
        }


        //foreach (var item in itemsToSpawn)
        //{
        //    int quantityToSpawn = Random.Range(item.minQuantityPerRoom, item.maxQuantityPerRoom);

        //    for (int i = 0; i < quantityToSpawn; i++)
        //    {
        //        {
        //            Vector2? spawnPosition = placementHelper.GetItemPlacementPositin(
        //                item.placementType,
        //                iterationsMax: 4,
        //                size: Vector2Int.one,
        //                addOffset: false
        //                );
        //            if (spawnPosition.HasValue)
        //            {
        //                Vector2Int vec2Ind = new Vector2Int((int)spawnPosition.Value.x, (int)spawnPosition.Value.y);
        //                var pos = BoardManager.Instance.CellToWorld(vec2Ind);
        //                GameObject spawnedItem = Instantiate(item.prefab, pos, Quaternion.identity, itemsParent.transform);
        //                Debug.Log($"Spawned item at position {spawnPosition.Value}");

        //                if (!item.isWalkable)
        //                {
        //                    BoxCollider2D collider = spawnedItem.AddComponent<BoxCollider2D>();
        //                    collider.isTrigger = false;
        //                }
        //            }
        //        }
        //    }
        //}




        //foreach (var item in placementHelper.GetAllFloors())
        //{
        //    Vector2Int vec2Ind = new Vector2Int((int)item.x, (int)item.y);
        //    var pos = BoardManager.Instance.CellToWorld(vec2Ind);
        //    GameObject spawnedItem = Instantiate(itemsToSpawn[0].prefab, pos, Quaternion.identity, itemsParent.transform);
        //}
    }

    public void SpawnBiggerObstacleInOpenSpace(ItemPlacementHelper placementHelper)
    {
        Debug.Log("Spawning bigger obstacle in 'OpenSpace'");

        if (BigerObstacleToSpawn == null || BigerObstacleToSpawn.Count == 0)
        {
            Debug.LogWarning("No obstacle newar wall to spawn!");
            return;
        }

        var openSpacePosition = placementHelper.GetTilesByType(PlacementType.OpenSpace).ToList();

        var corridorPositions = MapData.Instance.Corridors;
        openSpacePosition.RemoveAll(pos => corridorPositions.Contains(pos));

        foreach (var BigObstacle in BigerObstacleToSpawn)
        {
            int quantityToSpawn = Random.Range(BigObstacle.minQuantityPerRoom, BigObstacle.maxQuantityPerRoom);

            for (int i = 0; i < quantityToSpawn; i++)
            {
                int randomIndex = Random.Range(0,openSpacePosition.Count);
                Vector2Int spawnPosition = openSpacePosition[randomIndex];

                var worldPosition = BoardManager.Instance.CellToWorld(spawnPosition);

                GameObject spawnedObstacle = Instantiate(BigObstacle.prefab, worldPosition, Quaternion.identity, itemsParent.transform);

                if (!BigObstacle.isWalkable)
                {
                
                    BoxCollider2D collider = spawnedObstacle.AddComponent<BoxCollider2D>();
                    collider.isTrigger = false;
                }

                openSpacePosition.RemoveAt(randomIndex);
                placementHelper.occupiedTiles.Add(spawnPosition);

                if (openSpacePosition.Count == 0) break;
            }
        }
    }
}