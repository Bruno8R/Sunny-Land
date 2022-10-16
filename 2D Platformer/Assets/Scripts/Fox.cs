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

    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isCrouching = false;

    [SerializeField] bool facingRigth = true;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;

    Collider2D standingCollider;

    [SerializeField] Transform groundCheckCollider; //  to assign in the inspector the the object witch will check for collision
    [SerializeField] LayerMask groundLayer; // to assign in the inspector the "Ground" Layer
    const float groundCheckRadius = 0.2f; // set the radius the with the ground collision will check
    

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
    }

    void StopPlayer()
    {
        myRigidbody.velocity = new Vector2(0f, 0f);
    }

    void Movement()
    {
        myAnimator.SetFloat("xVelocity", Mathf.Abs(myRigidbody.velocity.x));
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);

        #region Walk & Run
        
        float moveSpeed = walkSpeed;
        if (isRunning){moveSpeed = runSpeed;}

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime * 100, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
            
        #endregion

        #region Crouch & Jump
        if(isGrounded){
            #region Jump
            if (isJumping)
            {
                myRigidbody.velocity += new Vector2(0f, jumpForce);
                myAnimator.SetBool("Jump", true);
                isJumping = false;
            }
            // set the "yVelocity" parameter in Unity Animator for the Jumping Blend Tree
            //myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
            #endregion

            // if we presse Crouch we disable the standing collider
            #region Crouch
            if (isCrouching){
                myAnimator.SetBool("Crouch", true);
                StopPlayer();
            }
            else myAnimator.SetBool("Crouch", false);
            #endregion
        }
        #endregion
    }

    void FlipSprite()
    {
        if (facingRigth && moveInput.x < 0)
        {
            transform.localScale = new Vector3 (-1, 1, 1);
            facingRigth = false;
        }
        else if(!facingRigth && moveInput.x > 0){
            transform.localScale = new Vector3 (1, 1, 1);
            facingRigth = true;
        }
    }
    
    void GroundCheck()
    {
        isGrounded = false;// disable the flag before it checks
        
        // get an array with all the colliders that are overlapping with the GroundCheck object in the 
        // ground/platform that are in the "Ground" Layer in the set radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        // if the length of the array is > 0 there is collision
        if (colliders.Length > 0){isGrounded = true;} // grounded

        // as long as we are grounder the "isJumping" bool in the animator is disabled
        myAnimator.SetBool("Jump", !isGrounded);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // @desc get run input
    void OnRun(InputValue value) {
        if (value.Get<float>() > 0){isRunning = true;}
        else {isRunning = false;}
    }

    void OnCrouch(InputValue value)
    {
        if (value.Get<float>() > 0){isCrouching = true;}
        else {isCrouching = false;}
    }

    // @desc check if the player is touching the ground:
    void OnJump(InputValue value)
    {
        if (!isGrounded){return;}
        if (value.isPressed){isJumping = true;}
    }
}
