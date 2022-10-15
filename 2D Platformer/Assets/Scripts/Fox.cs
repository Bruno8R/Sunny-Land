using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fox : MonoBehaviour
{
    //[SerializeField] 
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isRunning = false;

    
    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;


    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
        FlipSprite();


    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        float moveSpeed = walkSpeed;
        if (isRunning){moveSpeed = runSpeed;}
        
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime * 100, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed){transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);}
    }

    // @desc get run input
    void OnRun(InputValue value) {
        if (value.Get<float>() > 0){isRunning = true;}
        else {isRunning = false;}
    }
}
