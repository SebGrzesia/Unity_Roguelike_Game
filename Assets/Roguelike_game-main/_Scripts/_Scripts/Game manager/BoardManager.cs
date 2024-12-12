using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static BoardManager instance;

    public AbstractDungeonGenerator generator;
    public RoomFirstDungeonGenerator roomFirstDungeonGenerator;
    public GameObject playerPrefab;
    private PlayerController playerInstance;
    private Grid m_Grid;
    public Vector2Int spawnTile;

    public HealthDisplay healthDisplay;

    private Dictionary<Vector2Int, TitleProperties> tileProperties = new Dictionary<Vector2Int, TitleProperties>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_Grid = GetComponentInChildren<Grid>();
        generator.GenerateDungeon();

        GameObject playerObject = Instantiate(playerPrefab);
        playerInstance = playerObject.GetComponent<PlayerController>();
        playerInstance.SpawnPlayer(this, spawnTile);

        var playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (healthDisplay != null)
        {
            healthDisplay.SetPlayerHealth(playerHealth);
        }
    }

    public Vector3 CellToWorld(Vector2Int cellInde)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellInde);
    }

    public void SetSpawnTile(Vector2Int spawnTile)
    {
        this.spawnTile = spawnTile;
    }

    public bool TryGetTileProperties(Vector2Int position, out TitleProperties properties)
    {
        return tileProperties.TryGetValue(position, out properties);
    }

    public void InitializeTiles(Dictionary<Vector2Int, TitleProperties> tiles)
    {
        tileProperties = tiles;
    }

    public void ClearBoardData()
    {
        tileProperties.Clear();

        Debug.Log("Board Manager Has been cleared");
    }
}
