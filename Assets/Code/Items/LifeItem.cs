using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : Item
{
    public float m_Life;

    public override void Pick(FPSPlayerControllerV1 Player)
    {
        if (Player.GetLife() < 5.0f)
        {
            Player.AddLife(m_Life);
            gameObject.SetActive(false);
        }
    }
}
