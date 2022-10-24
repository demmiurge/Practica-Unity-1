using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<CollectibleItem> m_CollectibleItem;

    public void AddItemOnInventory(CollectibleItem Item)
    {
        bool l_WasAdded = false;

        // Check the existence of the item and if it can be accumulated
        foreach (CollectibleItem l_Item in m_CollectibleItem)
        {
            if (Item.m_ItemName == l_Item.m_ItemName && Item.m_IsStackable && l_Item.m_IsStackable)
            {
                l_Item.m_Amount++;
                l_WasAdded = true;
            }
        }

        // If it is not cumulative we add it as something new to the list
        if (l_WasAdded == false) m_CollectibleItem.Add(Item);
    }

    public bool CheckItemExistsByName(string ItemName)
    {
        return m_CollectibleItem.Any(l_Item => ItemName == l_Item.m_ItemName);
    }

    public bool CheckItemExistsByNameAndQuantity(string ItemName, int ItemAmount)
    {
        Debug.Log("TENGO? " + m_CollectibleItem.Count()); 

        foreach (var lItem in m_CollectibleItem)
        {
            if (ItemName == lItem.m_ItemName && ItemAmount <= lItem.m_Amount)
            {
                Debug.Log("Name " + lItem.m_ItemName);
                Debug.Log("AMOUNT " + lItem.m_Amount);
                return true;
            }
        }

        return false;
    }

    public void ConsumeItems(string ItemName, int ItemAmount)
    {
        for (int i = 0; i < m_CollectibleItem.Count; i++)
        {
            if (ItemName == m_CollectibleItem[i].m_ItemName && ItemAmount <= m_CollectibleItem[i].m_Amount)
                m_CollectibleItem[i].m_Amount -= ItemAmount;

            if (m_CollectibleItem[i].m_Amount == 0) 
                m_CollectibleItem.RemoveAt(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CollectibleItem = new List<CollectibleItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
