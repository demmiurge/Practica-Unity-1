using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform m_Target;

    void LateUpdate()
    {
        transform.LookAt(m_Target);
    }
}
