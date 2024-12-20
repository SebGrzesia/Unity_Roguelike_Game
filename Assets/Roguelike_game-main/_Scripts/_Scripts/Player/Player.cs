using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Animator animator;

    private PlayerMovment playerMover;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput => pointerInput;

    private WeaponParent weaponParent;

    [SerializeField]
    private InputActionReference movement, attack, pointerPosition;

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext context)
    {
        if ( weaponParent == null )
        {
            Debug.LogError("Weapon parrent is null", gameObject);
            return;
        }
        weaponParent.Attack();
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        playerMover = GetComponent<PlayerMovment>();
    }

    private void AnimatePlayer()
    {
        // Oblicz kierunek wskaźnika w stosunku do gracza
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;

        // Obrót postaci na podstawie kierunku wskaźnika
        if (lookDirection.x < 0) // Jeśli wskaźnik po lewej stronie
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0); // Obrót w lewo
        }
        else // Jeśli wskaźnik po prawej stronie
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Obrót w prawo
        }

        // Sprawdzenie, czy gracz się porusza
        bool isMoving = movementInput.sqrMagnitude > 0;

        // Ustawienie parametru "Moving" w Animatorze
        animator.SetBool("Moving", isMoving);
    }


    private void Update()
    {
        pointerInput = GetPointerInput();
        weaponParent.Pointerposition = pointerInput;
        movementInput = movement.action.ReadValue<Vector2>();

        playerMover.MovementInput = movementInput;

        AnimatePlayer();
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }


    //private void AnimateCharacter()
    //{
    //    if (animator == null) return;

    //    // Kierunek patrzenia (od gracza do wskaźnika)
    //    Vector2 lookDirection = pointerInput - (Vector2)transform.position;
    //    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

    //    // Obrót postaci w kierunku wskaźnika
    //    transform.rotation = Quaternion.Euler(0, -angle, 0);

    //    // Ustawianie animacji biegania
    //    bool isMoving = movementInput.sqrMagnitude > 0; // Sprawdzamy, czy gracz się porusza
    //    animator.SetBool("IsMoving", isMoving);
    //}

}
