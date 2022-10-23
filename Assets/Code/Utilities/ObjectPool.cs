using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool m_SharedInstance;
    public List<GameObject> m_PooledObjects;
    public GameObject m_ObjectToPool;
    public int m_AmountToPool;

    void Awake()
    {
        m_SharedInstance = this;
    }

    void Start()
    {
        m_PooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < m_AmountToPool; i++)
        {
            tmp = Instantiate(m_ObjectToPool);
            tmp.SetActive(false);
            m_PooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_AmountToPool; i++)
        {
            if (!m_PooledObjects[i].activeInHierarchy)
            {
                return m_PooledObjects[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
