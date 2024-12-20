using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovmentInput, OnPointerInput;
    public UnityEvent OnAttack;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 4, attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1;
    private float passedTime = 1;

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(player.position, transform.position);
        if(distance < attackDistanceThreshold)
        {
            //attack
            OnMovmentInput?.Invoke(Vector2.zero);
            if(passedTime >= attackDelay)
            {
                passedTime = 0;
                OnAttack?.Invoke();
            }
            else
            {
                //chasing the player
                Vector2 direction = player.position - transform.position;
                OnMovmentInput?.Invoke(direction.normalized);
            }
        }
        if(passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }
}
