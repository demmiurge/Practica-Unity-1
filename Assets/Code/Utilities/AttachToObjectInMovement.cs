using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObjectInMovement : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            Debug.Log("Enter platform");
            transform.parent = other.transform;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            Debug.Log("Exit platform");
            transform.parent = null;
        }
    }
}
