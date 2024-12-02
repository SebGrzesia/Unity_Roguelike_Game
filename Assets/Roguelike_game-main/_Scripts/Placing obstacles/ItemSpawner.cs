using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<SpawnableItem> itemsToSpawn;
    public GameObject itemsParent;

    public void SpawnItems(ItemPlacementHelper placementHelper)
    {
        Debug.Log("Spawning items");
        foreach (var item in itemsToSpawn)
        {
            int quantityToSpawn = Random.Range(item.minQuantityPerRoom, item.maxQuantityPerRoom);

            for (int i = 0; i < quantityToSpawn; i++)
            {
                {
                    Vector2? spawnPosition = placementHelper.GetItemPlacementPositin(
                        item.placementType,
                        iterationsMax: 4,
                        size: Vector2Int.one,
                        addOffset: false
                        );
                    if(spawnPosition.HasValue)
                    {
                        GameObject spawnedItem = Instantiate(item.prefab,spawnPosition.Value,Quaternion.identity, itemsParent.transform);
                        Debug.Log($"Spawned item at position {spawnPosition.Value}");

                        if (!item.isWalkable )
                        {
                            BoxCollider2D collider = spawnedItem.AddComponent<BoxCollider2D>();
                            collider.isTrigger = false;
                        }
                    }
                }
            }
        }
    }
}
