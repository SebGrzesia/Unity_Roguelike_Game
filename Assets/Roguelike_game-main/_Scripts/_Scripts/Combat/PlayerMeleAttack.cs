using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public float startTimeBetweenAttack;
    private bool canAttack = true;

    public Animator animator;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    //private void Update()
    //{
    //    if (canAttack && Input.GetKey(KeyCode.Mouse0))
    //    {
    //        StartCoroutine(Attack());
    //    }
    //}

    //private IEnumerator Attack()
    //{
    //    canAttack = false;

    //    // Trigger the attack animation
    //    animator.SetTrigger("MeleeAttack");

    //    // Detect enemies in range and apply damage
    //    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
    //    for (int i = 0; i < enemiesToDamage.Length; i++)
    //    {
    //        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
    //    }

    //    // Wait for the specified time before allowing another attack
    //    yield return new WaitForSeconds(startTimeBetweenAttack);

    //    canAttack = true;
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
