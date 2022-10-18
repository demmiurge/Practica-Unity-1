using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public CollectibleItem m_WhatItemAmI;

    public CollectibleItem GetItem() => m_WhatItemAmI;
}
