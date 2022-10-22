using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    public Transform detectionPoint; // DetectionPoint
    private const float detectionRadius = 0.2f; // DetectionRadius
    public LayerMask detectionLayer; // Detection layer
    public GameObject detectedObject; // cached Trigger Object

    // @desc get the interact input
    void OnInteract(InputValue value)
    {
        if(DetectObject())
        {
            if(value.isPressed)
            {
                detectedObject.GetComponent<Items>().Interact();
            }   
        }
    }

   bool DetectObject()
   {
        bool detected;
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if(obj == null)
        {
            detectedObject = null;
            detected =  false;
        }
        else
        {
            detectedObject = obj.gameObject;
            detected = true;
        }
        return detected;
   }
}
