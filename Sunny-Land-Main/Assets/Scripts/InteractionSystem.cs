using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class InteractionSystem : MonoBehaviour
{

    
    public Transform detectionPoint; //DetectionPoint
    private const float detectionRadius = 0.2f; //DetectionRadius
    public LayerMask detectionLayer; //Detection layer

    //bool isInteracting = false;

    void Update()
    {

    }

   // bool InteractInput()

   bool DetectObject()
   {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
   }

    // @desc get the interact input
    void OnInteract(InputValue value)
    {
        if(DetectObject())
        {
            if(value.isPressed)
            {
                Debug.Log("INTERACTING");
            }
        }
    }

}
