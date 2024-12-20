using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int minEnemyPerRoom = 3;
    public int maxEnemyPerRoom = 5;

    public void SpawnEnemies(ItemPlacementHelper placementHelper)
    {
        foreach (var room in MapData.Instance.Rooms)
        {
            int enemyToSpawn = Random.Range(minEnemyPerRoom, maxEnemyPerRoom);
            List<Vector2Int> roomTiles = new List<Vector2Int>(room);

            for (int i = 0; i < enemyToSpawn; i++)
            {
                if (roomTiles.Count == 0) break;
                int randomIndex = Random.Range(0, roomTiles.Count);
                Vector2Int spawnPosition = roomTiles[randomIndex];

                //check if thiles isn't occupied
                if (!placementHelper.occupiedTiles.Contains(spawnPosition))
                {
                    GameObject enemy = Instantiate(enemyPrefab, AdjustPosition(spawnPosition), Quaternion.identity);
                    enemy.transform.parent = transform;
                    placementHelper.occupiedTiles.Add(spawnPosition);
                }
                roomTiles.RemoveAt(randomIndex);
            }
        }
    }

    private Vector3 AdjustPosition(Vector2Int gridPosition)
    {
        Vector3 adjustedPosition = new Vector3(gridPosition.x + 0.5f, gridPosition.y + 0.5f, 0);
        return adjustedPosition;
    }

    public void DestroyAllChildOfThisObject()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}