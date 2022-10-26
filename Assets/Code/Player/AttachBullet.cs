using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBullet : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Target")
        {
            transform.parent = other.transform;
        }
    }
}
