using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float maxSpeed = 5, acceleration = 50, deacceleration = 100;

    [SerializeField]
    private float currentSpeed = 0;

    private Vector2 oldMovementInput;

    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (MovementInput.magnitude > 0 && currentSpeed >=0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime ;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb.velocity = oldMovementInput * currentSpeed;
    }
}


//private float horizontal;
//    private float vertical;
//    public float speed;
//    private bool isFacingRight = true;
//    private Vector2 moveDirection;
//    private Animator animator;

//    public Rigidbody2D rb;

//    private void Awake()
//{
//    animator = GetComponent<Animator>();
//}

//void Update()
//{
//    ProcessInputs();
//    FlipTowardsCursor();

//    if (animator != null)
//    {
//        bool isMoving = moveDirection.sqrMagnitude > 0;
//        animator.SetBool("Moving", isMoving);
//    }
//}

//private void FixedUpdate()
//{
//    Move();
//}

//void ProcessInputs()
//{
//    horizontal = Input.GetAxisRaw("Horizontal");
//    vertical = Input.GetAxisRaw("Vertical");

//    moveDirection = new Vector2(horizontal, vertical).normalized;
//}

//void Move()
//{
//    float adjustedSpeed = speed;

//    if (isFacingRight && moveDirection.x < 0)
//    {
//        adjustedSpeed = speed * 0.5f;
//    }

//    else if (!isFacingRight && moveDirection.x > 0)
//    {
//        adjustedSpeed = speed * 0.5f;
//    }
//    rb.velocity = new Vector2(moveDirection.x * adjustedSpeed, moveDirection.y * speed);
//}


//private void FlipTowardsCursor()
//{
//    Vector3 mousePosition = Input.mousePosition;

//    Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

//    if (mousePosition.x < playerScreenPosition.x && isFacingRight)
//    {
//        Flip();
//    }
//    else if (mousePosition.x > playerScreenPosition.x && !isFacingRight)
//    {
//        Flip();
//    }
//}


//private void Flip()
//{
//    isFacingRight = !isFacingRight;
//    Vector3 localScale = transform.localScale;
//    localScale.x *= -1f;
//    transform.localScale = localScale;
//}