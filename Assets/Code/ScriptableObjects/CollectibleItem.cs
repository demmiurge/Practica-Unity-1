using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class CollectibleItem : ScriptableObject
{
    public string m_ItemName;
    public bool m_IsStackable;
    public int m_Amount;
}