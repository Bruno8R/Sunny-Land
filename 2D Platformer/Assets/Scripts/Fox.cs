using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fox : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float runSpeed = 20f;
    [SerializeField] float jumpSpeed = 15f;
    
    Animator myAnimator;
    Rigidbody2D myRigidbody;
    PlayerInput playerInput;
    Vector2 moveInput;
    
    [SerializeField] Transform groundCheckCollider; //  to assign in the inspector the the object witch will check for collision
    [SerializeField] LayerMask groundLayer; // to assign in the inspector the "Ground" Layer 
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isCrouching = false;
    const float groundCheckRadius = 0.2f; 


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();  
        myAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        GroundCheck();
        Movement();
        FlipSprite();
        Jumping();
        Crouching();
    }

    // @desc execute player movement and the respective animation
   void Movement()
    {   
        if (isCrouching){return;} // check if the player is crouching
        if(!isJumping){
            Debug.Log((myRigidbody.velocity.y));
            float moveSpeed;

            // check if the player is running or walking and then assing the respective move speed
            if (!isRunning){moveSpeed = walkSpeed;}
            else{moveSpeed = runSpeed;}

            // move the player
            Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;

            // check if the player has horizontal speed, and if true animate
            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Moving", playerHasHorizontalSpeed);
        }
    }

    // @desc play jumping and falling animations
    void Jumping()
    {
        if (isJumping && isGrounded){myRigidbody.velocity += new Vector2(0f, jumpSpeed);}
        // set the "yVelocity" parameter in Unity Animator for the Jumping Blend Tree
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        isJumping = false;
    }

    //  @desc get Horizontal and Vertical input
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }  

    // @desc play crouching animation
    void Crouching()
    {
        // check if the player is crouching and animate the player
        if (isCrouching){myAnimator.SetBool("Crouch", true);}
        else myAnimator.SetBool("Crouch", false);
    }

    // @desc get Jump input and move(jump) the player
    public void Jump(InputAction.CallbackContext context)
    {   
        if (context.performed)
        {
            //myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            isJumping = true;
        }
    }

    // @desc get run input
    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed){isRunning = true;}
        else isRunning = false;
    }

    // @desc get crouch input
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed){isCrouching = true;}
        else isCrouching = false;
    }

    // @desc flip the player sprite
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed){transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);}
    }

    // @desc check if the player is touching the ground:
    // Checks if the GroundCheck objects inside the Player is colliding with the 2d collider in the 
    // ground/platform that are in the "Ground" Layer
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
}