using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObjectInMovement : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            transform.parent = other.transform;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            transform.parent = null;
        }
    }
}
