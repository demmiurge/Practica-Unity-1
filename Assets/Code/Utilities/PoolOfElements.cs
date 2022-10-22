using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOfElements : MonoBehaviour
{
    List<GameObject> m_ObjectsPool;
    int m_CurrentElement = 0;
    public PoolOfElements(int ElementsCount, GameObject Element)
    {
        m_ObjectsPool = new List<GameObject>();
        for (int i = 0; i < ElementsCount; ++i)
        {
            GameObject l_Element = GameObject.Instantiate(Element);
            l_Element.SetActive(false);
            m_ObjectsPool.Add(Element);
        }
        m_CurrentElement = 0;
    }

    public GameObject GetNextElement()
    {
        GameObject l_Element = m_ObjectsPool[m_CurrentElement];
        ++m_CurrentElement;
        if (m_CurrentElement >= m_ObjectsPool.Count)
        {
            m_CurrentElement = 0;
        }

        return l_Element;
    }
}
