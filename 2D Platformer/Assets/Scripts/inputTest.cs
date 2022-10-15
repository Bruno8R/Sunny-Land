using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputTest : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;

    Rigidbody2D rb;
    Animator myAnimator;
    PlayerInput playerInput;
    public Vector2 moveInput;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        Movement();
        FlipSprite();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            Debug.Log("jump");
            rb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Debug.Log("Move");
        moveInput = context.ReadValue<Vector2>();
    }


    /*public void Run(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            Debug.Log("Run" + context.phase);
        }
    }*/

    void Movement()
    {   
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        myAnimator.SetFloat("xVelocity", rb.velocity.x);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}
