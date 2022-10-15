using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float moveSpeed = 10f;

    Rigidbody2D rb;

    public inputTest input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        Jump();
        FlipSprite();
    }

    void Movement()
    {   
        Vector2 playerVelocity = new Vector2(input.moveInput.x * 10f, rb.velocity.y);
        rb.velocity = playerVelocity;
    }

    // dosen't work, the player only goes up
    void Jump()
    {
        rb.velocity += new Vector2(0f, jumpSpeed);
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

