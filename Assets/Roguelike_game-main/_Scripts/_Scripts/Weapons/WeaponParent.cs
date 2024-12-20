using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    //private string weapon;
    //public SpriteRenderer characterRenderer, weaponRenderer;

    public Vector2 Pointerposition { get; set; }

    public Animator animator;

    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Update()
    {
        //if (weapon == null)
        //{
        //    Debug.LogError("Weapon is null", gameObject);
        //    return;
        //}

        if (IsAttacking)
        {
            return;
        }
        Vector2 direction = (Pointerposition - (Vector2)transform.position).normalized;

        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if ( direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        //if ( transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        //{
        //    weaponRenderer.sortingOrder = characterRenderer.sortingOrder -1;
        //}
        //else
        //{
        //    weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        //}

        //weapon.Use();
        //weaponRotationStopped = true;
    }

    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position,radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position,radius))
        {
            Enemy health;
            if (health = collider.GetComponent<Enemy>())
            {
                health.GetHit(1,transform.parent.gameObject);
            }
        }
    }
}
