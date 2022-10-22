using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fox : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float coyoteJumpTime = 0.2f;
    [SerializeField] int totalJumps;
    [SerializeField] int avaibleJumps = 2;
    
    bool multipleJumps = true;
    bool isGrounded = false;
    bool isRunning = false;
    bool isCrouching = false;
    bool facingRigth = true;
    bool coyoteJump = false;
    public bool isInteracting = false;

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
        FlipSprite();
        StopPlayer();
        Movement();
    }

    void Movement(){
        float moveSpeed = walkSpeed;// the player is walking by default
        if (isRunning){moveSpeed = runSpeed;}// check if the player is running, if true change the move speed

        // check if the player has horizontal speed, and if true move the player
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime * 100, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;  

        // Animate the player
        myAnimator.SetFloat("xVelocity", Mathf.Abs(myRigidbody.velocity.x));// get player horizontal velocity
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);// get player vertical velocity
    }

    // @desc check if the player is touching the ground.........
    void GroundCheck()
    {
        bool wasGrounded = isGrounded; 
        isGrounded = false;// disable the flag before it checks
        
        // get an array with all the colliders that are overlapping with the GroundCheck object in the 
        // ground/platform that are in the "Ground" Layer in the set radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        // if the length of the array is > 0 there is collision so the player is on the ground
        if (colliders.Length > 0){ 
            isGrounded = true;
            if (!wasGrounded)
            {
                avaibleJumps = totalJumps;
                multipleJumps = false;
            }
        }
        else if (wasGrounded) // 
        {
            StartCoroutine(CoyoteJump());
        }

        // as long as we are grounded the "isJumping" bool in the animator is disabled
        myAnimator.SetBool("Jump", !isGrounded);
    }

    // @desc coroutine to set the time of the "coyote jump" effect
    IEnumerator CoyoteJump()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(coyoteJumpTime);
        coyoteJump = false;
    }

    // @desc flip the sprite direction
    void FlipSprite()
    {
        // if the sprite is facing right and the move input is to the left flip the sprite to the left
        if (facingRigth && moveInput.x < 0)
        {
            transform.localScale = new Vector3 (-1, 1, 1);
            facingRigth = false; 
        }
        // if the sprite is not facing right and the move input is to the rigth flip the sprite to the rigth
        else if(!facingRigth && moveInput.x > 0){
            transform.localScale = new Vector3 (1, 1, 1);
            facingRigth = true;
        }
    }

    // @desc stop the player movement
    void StopPlayer()
    {
        if (isCrouching){myRigidbody.velocity = new Vector2(0f, 0f);}//stop the player if he is crouching
    }

    // @desc get the movement input
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();// get input
    }

    // @desc get the run input
    void OnRun(InputValue value) {
        isRunning = value.isPressed;
    }

    // @desc get the crouch input
    void OnCrouch(InputValue value)
    {
        isCrouching = value.isPressed;
        if(isGrounded){
            standingCollider.enabled = !isCrouching;
            myAnimator.SetBool("Crouch", isCrouching);
        }
    }

    // @desc get the jump input
    void OnJump(InputValue value)
    {
        // allow jumps only when we made the first jump or when in "coyote jump time"
        if(value.isPressed){
            if(isGrounded)// if the player is grounded allow multiple jumps
            {
                multipleJumps = true;// only allow multiple jumps when we made the first jump
                avaibleJumps--; 
                myRigidbody.velocity = Vector2.up * jumpForce;
                myAnimator.SetBool("Jump", true);
            }
            else if(coyoteJump)// if the player is in coyote jump time allow jumping and double jumping
            {
                multipleJumps = true;// only allow multiple jumps with we made the first jump
                avaibleJumps--;
                myRigidbody.velocity = Vector2.up * jumpForce;
                myAnimator.SetBool("Jump", true);
            }
            else if (multipleJumps && avaibleJumps > 0)// jump if we can doublejump and have remaning jumps
            {
                avaibleJumps--;
                myRigidbody.velocity = Vector2.up * jumpForce;
                myAnimator.SetBool("Jump", true);
            }
        }
    }
}
