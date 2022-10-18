using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public Inventory m_Inventory;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            CollectibleItem l_Item = other.GetComponent<ItemType>().GetItem();
            m_Inventory.AddItemOnInventory(l_Item);
        }
    }
}
