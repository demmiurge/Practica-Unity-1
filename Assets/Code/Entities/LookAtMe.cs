using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform m_Target;

    void Start()
    {
        if (!m_Target && GameController.GetGameController().GetPlayer().transform) m_Target = GameController.GetGameController().GetPlayer().transform;
    }

    void LateUpdate()
    {
        transform.LookAt(m_Target);
    }
}
