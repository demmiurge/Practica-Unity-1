using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<CollectibleItem> m_CollectibleItem;

    public void AddItemOnInventory(CollectibleItem Item)
    {
        bool l_WasAdded = false;

        // Check the existence of the item and if it can be accumulated
        foreach (CollectibleItem item in m_CollectibleItem)
        {
            if (Item.m_ItemName == item.m_ItemName && Item.m_IsStackable && item.m_IsStackable)
            {
                item.m_Amount++;
                l_WasAdded = true;
            }
        }

        // If it is not cumulative we add it as something new to the list
        if (l_WasAdded == false) m_CollectibleItem.Add(Item);
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
