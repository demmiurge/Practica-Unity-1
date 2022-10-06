using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider l_Collider)
    {
        if (l_Collider.tag == "DeadZone")
        {
            transform.position = GameObject.FindObjectOfType<CheckpointManager>().GetLatestCheckpointReached().position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
