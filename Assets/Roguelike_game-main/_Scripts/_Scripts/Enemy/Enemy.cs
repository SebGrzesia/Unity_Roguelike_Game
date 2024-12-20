using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    private Animator animator;

    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    public bool isDead = false;

    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

     public void GetHit(int ammount, GameObject sender)
    {
        if (isDead)
        {
            return;
        }
        if (sender.layer == gameObject.layer)
        {
            return;
        }

        currentHealth -= ammount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnHitWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }


    //public int health;
    //public float speed;
    //private Rigidbody2D enemyRb;
    //private GameObject player;

    //private Animator animator;




    //private void Start()
    //{
    //    //player = GameObject.FindGameObjectWithTag("Player");
    //    enemyRb = GetComponent<Rigidbody2D>();
    //    animator = GetComponent<Animator>();
    //    //animator.SetBool("isRunning", true);

    //}


    //private void FixedUpdate()
    //{
    //    if(player != null)
    //    {
    //        Vector2 direction = (player.transform.position - transform.position).normalized;

    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f);

    //        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
    //        {
    //            direction += new Vector2(-direction.y, direction.x);
    //        }

    //        enemyRb.MovePosition((Vector2)transform.position + direction * speed* Time.fixedDeltaTime);
    //    }

    //    if (health <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void TakeDamage(int damage)
    //{
    //    //add a hurth sound
    //    health -= damage;
    //    Debug.Log("Enemy got damage");
    //}
}
