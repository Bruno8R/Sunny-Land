using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class InteractionSystem : MonoBehaviour
{

    private Fox fox;
    
    public Transform detectionPoint; //DetectionPoint
    private const float detectionRadius = 0.2f; //DetectionRadius
    public LayerMask detectionLayer; //Detection layer

    //bool isInteracting = false;

    void Update()
    {
        if(DetectObject())
        {
            if(fox.isInteracting)
            {
                Debug.Log("INTERACTING");
            }
        }
    }

   // bool InteractInput()

   bool DetectObject()
   {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
   }


}
