using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBulletInObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            other.transform.parent = transform;
        }
    }
}
