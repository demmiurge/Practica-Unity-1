using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Space(5)]
    [Header("General checkpoint parameters")]
    [Space(10)]
    [Tooltip("Horizontal rotation speed")]
    public int m_CheckpointNumber;

    public int GetNumberOfCheckpoint() => m_CheckpointNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider l_Collider)
    {
        if (l_Collider.tag == "Player")
        {
            Debug.Log("Me toco el jugador");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
