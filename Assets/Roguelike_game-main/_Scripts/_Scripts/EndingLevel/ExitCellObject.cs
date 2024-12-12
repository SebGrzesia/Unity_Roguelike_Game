using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : MonoBehaviour
{
    public RoomFirstDungeonGenerator roomFirstDungeonGenerator;

    private void Awake()
    {
        // Jeśli nie przypisano ręcznie w inspektorze, znajdź komponent w scenie
        if (roomFirstDungeonGenerator == null)
        {
            roomFirstDungeonGenerator = FindObjectOfType<RoomFirstDungeonGenerator>();
            if (roomFirstDungeonGenerator == null)
            {
                Debug.LogError("RoomFirstDungeonGenerator not found in the scene!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.keyToNextLvl)
            {
                PlayerEntered(collision.gameObject);
            }
            else
            {
                Debug.Log("Player don't have a key");
            }
        }
    }

    private void PlayerEntered(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.keyToNextLvl = false;
        }
        Debug.Log("player Entered with key");

        RefreshLevel(player);
    }

    private void RefreshLevel(GameObject player)
    {
        TilemapVisualizer tilemapVisualizer = FindObjectOfType<TilemapVisualizer>();
        if (tilemapVisualizer != null)
        {
            tilemapVisualizer.Clear();
        }
        RemoveAllObstacles();
        GenerateNextDungeonLevel();
        MovePlayerToStartLevel(player);
    }

    private void MovePlayerToStartLevel(GameObject player)
    {
        if (BoardManager.Instance == null)
        {
            Debug.LogError("BoardManager.Instance is null! Make sure BoardManager is in the scene.");
            return;
        }

        Vector2Int startTile = BoardManager.Instance.spawnTile;
        Vector3 startPosition = BoardManager.Instance.CellToWorld(startTile);

        player.transform.position = startPosition;

        Debug.Log($"Player moved to start position: {startTile}");
    }

    private void GenerateNextDungeonLevel()
    {
        if (roomFirstDungeonGenerator == null)
        {
            Debug.LogError("RoomFirstDungeonGenerator is not assigned or found in the scene!");
            return;
        }
        roomFirstDungeonGenerator.GenerateNewLevel();
    }

    private void RemoveAllObstacles()
    {
        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
    }
}
