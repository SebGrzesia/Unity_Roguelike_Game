using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite fullHeart;
    public Image[] hearts;

    public PlayerHealth playerHealth;

    private void Start()
    {
        hearts = GetComponentsInChildren<Image>();
    }

    public void SetPlayerHealth(PlayerHealth health)
    {
        playerHealth = health;
    }

    void Update()
    {
        int health = playerHealth.health;
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
                hearts[i].sprite = i < health ? fullHeart : null;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
