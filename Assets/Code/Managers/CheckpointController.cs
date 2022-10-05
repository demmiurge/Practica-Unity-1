using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    bool m_IsChecked;

    [Space(5)]
    [Header("General checkpoint parameters")]
    [Space(10)]
    [Tooltip("Horizontal rotation speed")]
    public int m_CheckpointNumber;

    public delegate void PlayerReachedToCheckpoint(CheckpointController l_CheckpointController);
    public static event PlayerReachedToCheckpoint m_PlayerReachedToCheckpoint;

    public int GetNumberOfCheckpoint() => m_CheckpointNumber;
    public void ActiveIsChecked() => m_IsChecked = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider l_Collider)
    {
        if (l_Collider.tag == "Player")
        {
            if (m_IsChecked == false)
            {
                m_PlayerReachedToCheckpoint?.Invoke(this); // Need to refact
            }

            m_IsChecked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
