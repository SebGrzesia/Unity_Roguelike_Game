using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite fullHeart;
    public Image[] hearts;

    public PlayerHealth playerHealth;
    private Color color;

    private void Start()
    {
        hearts = GetComponentsInChildren<Image>();
        color = Color.white;
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
                //hearts[i].sprite = i < health ? fullHeart : null;
                color.a = i < health ? 1 : 0 ;
                hearts[i].color = color;
            }
            else
            {
                //hearts[i].enabled = false;
                hearts[i].color = color;
                color.a = 0;
            }
        }
    }
}
