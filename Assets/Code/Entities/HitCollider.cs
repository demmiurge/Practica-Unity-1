using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public float m_DamageApplied;

    public Entity m_Entity;

    public void Hit()
    {
        m_Entity.Hit(m_DamageApplied);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
