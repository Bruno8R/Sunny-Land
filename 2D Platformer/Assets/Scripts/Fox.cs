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

    
    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;

    [SerializeField] Transform groundCheckCollider; //  to assign in the inspector the the object witch will check for collision
    [SerializeField] LayerMask groundLayer; // to assign in the inspector the "Ground" Layer
    const float groundCheckRadius = 0.2f; // set the radius the with the ground collision will check
    
    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isCrouching = false;

    

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
        Movement();
        FlipSprite();
        GroundCheck();
       // JumpPlayer();

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Movement()
    {
        #region Jump

        if (isJumping && isGrounded)
        {
            myRigidbody.velocity += new Vector2(0f, jumpForce);
            myAnimator.SetBool("Jump", true);
            isJumping = false;
        }
        // set the "yVelocity" parameter in Unity Animator for the Jumping Blend Tree
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        #endregion

        #region Walk & Run
            
        float moveSpeed = walkSpeed;
        if (isRunning){moveSpeed = runSpeed;}

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime * 100, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        
        myAnimator.SetFloat("xVelocity", Mathf.Abs(myRigidbody.velocity.x));
        #endregion
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

    void OnCrouch(InputValue value) {
        if (value.Get<float>() > 0){isCrouching = true;}
        else {isCrouching = false;}
    }

    // @desc check if the player is touching the ground:
    void GroundCheck()
    {
        isGrounded = false;// disable the flag before it checks
        
        // get an array with all the colliders that are overlapping with the GroundCheck object in the 
        // ground/platform that are in the "Ground" Layer in the set radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        // if the length of the array is > 0 there is collision
        if (colliders.Length > 0) // grounded
        {
            isGrounded = true;
            //isJumping = false;
        }

        // as long as we are grounder the "isJumping" bool in the animator is disabled
        myAnimator.SetBool("Jump", !isGrounded);
    }

    void OnJump(InputValue value)
    {
        if (!isGrounded){return;}

        if (value.isPressed)
        {
            //isGrounded = false;
            //myRigidbody.velocity += new Vector2(0f, jumpForce);
            //myAnimator.SetBool("isJumping", true);
            isJumping = true;
        }
        
    }


}
