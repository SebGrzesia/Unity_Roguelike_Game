using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableItem
{
    public GameObject prefab;
    public PlacementType placementType;
    public bool isWalkable;
    public int minQuantityPerRoom = 0;
    public int maxQuantityPerRoom = 7;
}
