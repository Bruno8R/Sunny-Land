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

    [SerializeField] int totalJumps;
    [SerializeField] int avaibleJumps = 2;
    [SerializeField] bool multipleJumps = true;

    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isCrouching = false;

    [SerializeField] bool facingRigth = true;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;

    CapsuleCollider2D standingCollider;

    [SerializeField] Transform groundCheckCollider; //  to assign in the inspector the the object witch will check for collision
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer; // to assign in the inspector the "Ground" Layer
    const float groundCheckRadius = 0.2f; // set the radius the with the ground collision will check
    const float overheadCheckRadius = 0.2f;
    

    void Awake() 
    {
        avaibleJumps = totalJumps;
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        standingCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        
        GroundCheck();
    }

    void FixedUpdate()
    {
        Movement();
        Jump();
        Crouching();
        FlipSprite();
    }

    void Movement()
    {
        myAnimator.SetFloat("xVelocity", Mathf.Abs(myRigidbody.velocity.x));
        float moveSpeed = walkSpeed;
        if (isRunning){moveSpeed = runSpeed;}

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime * 100, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;     
    }

    void Crouching()
    {
        if(isGrounded){
/*          if (!isCrouching)
            {
                if(Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
                    isCrouching = true;
            }  */
            
            standingCollider.enabled = !isCrouching;
            StopPlayer(isCrouching);
            myAnimator.SetBool("Crouch", isCrouching);
        }
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;// disable the flag before it checks
        
        // get an array with all the colliders that are overlapping with the GroundCheck object in the 
        // ground/platform that are in the "Ground" Layer in the set radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        // if the length of the array is > 0 there is collision
        if (colliders.Length > 0){ // grounded
            isGrounded = true;
            if (!wasGrounded)
            {
                avaibleJumps = totalJumps;
                multipleJumps = false;
                isJumping = false;
            }
        }

        // as long as we are grounder the "isJumping" bool in the animator is disabled
        myAnimator.SetBool("Jump", !isGrounded);
    }

    void Jump()
    {
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        if(isJumping){
            if(isGrounded)
            {
                multipleJumps = true; // only allow multiple jumps with we made the first jump
                avaibleJumps--;
                
                myRigidbody.velocity = Vector2.up * jumpForce;
                myAnimator.SetBool("Jump", true);
                isJumping = false;
            }
            else if (multipleJumps && avaibleJumps > 0)
            {
                avaibleJumps--;
                myRigidbody.velocity = Vector2.up * jumpForce;
                myAnimator.SetBool("Jump", true);
                isJumping = false;
            }
        }
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

    void StopPlayer(bool value)
    {
        if (value){myRigidbody.velocity = new Vector2(0f, 0f);}
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // @desc get run input
    void OnRun(InputValue value) {
        isRunning = value.Get<float>() > 0;
    }

    void OnCrouch(InputValue value)
    {
        isCrouching = value.Get<float>() > 0;
    }

    // @desc check if the player is touching the ground:
    void OnJump(InputValue value)
    {
        isJumping = value.isPressed;
    }
}
