using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerTakeKey(collision.gameObject);
        }
    }

    private void PlayerTakeKey(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.keyToNextLvl = true;
        }
        GameObject.Destroy(gameObject);
    }
}
