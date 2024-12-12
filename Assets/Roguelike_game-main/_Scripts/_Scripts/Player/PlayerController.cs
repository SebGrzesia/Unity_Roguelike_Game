using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_board;
    private Vector2Int playerCellActualPosition;


    public void SpawnPlayer(BoardManager boardManager, Vector2Int spawnTile)
    {
        m_board = boardManager;
        playerCellActualPosition = spawnTile;

        transform.position = m_board.CellToWorld(spawnTile);
    }

    private void Update()
    {
        //Vector2Int newCellTarget = playerCellActualPosition;
        //bool hasMoved = false;

        //if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        //{
        //    newCellTarget.y += 1;
        //    hasMoved = true;
        //}
        //else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        //{
        //    newCellTarget.y -= 1;
        //    hasMoved = true;
        //}
        //else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        //{
        //    newCellTarget.x += 1;
        //    hasMoved = true;
        //}
        //else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        //{
        //    newCellTarget.x -= 1;
        //    hasMoved = true;
        //}
        //if (hasMoved)
        //{
        //    if (CanMoveToTile(newCellTarget))
        //    {
        //        playerCellActualPosition = newCellTarget;
        //        transform.position = m_board.CellToWorld(newCellTarget);
        //    }
        //}

    }

    private bool CanMoveToTile(Vector2Int targetCell)
    {
        if (m_board.TryGetTileProperties(targetCell, out TitleProperties tileProperties))
        {
            return tileProperties.Passable;
        }
        return false;
    }
}
