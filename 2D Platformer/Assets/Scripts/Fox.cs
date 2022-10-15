using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fox : MonoBehaviour
{
    [SerializeField] float walkSpeed = 500f;
    [SerializeField] float jumpForce = 5f;
    
    Rigidbody2D myRigidbody;
    Vector2 moveInput;

    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * walkSpeed * Time.fixedDeltaTime, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }
}
