using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform m_Target;

    void Start()
    {
        if (!m_Target && GameObject.FindGameObjectWithTag("Player")) m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(m_Target);
    }
}
