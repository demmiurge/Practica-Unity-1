using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private CheckpointController[] m_Checkpoints;
    //List<CheckpointController> m_Checkpoints = new List<CheckpointController>();

    // Start is called before the first frame update
    void Start()
    {
        m_Checkpoints = GameObject.FindObjectsOfType<CheckpointController>();
        Debug.Log("TOTAL: " + m_Checkpoints.Length);

        m_Checkpoints = m_Checkpoints.OrderBy(l_Checkpoint => l_Checkpoint.GetNumberOfCheckpoint()).ToArray();

        foreach (CheckpointController l_Checkpoint in m_Checkpoints)
        {
            print("Checkpoint: " + l_Checkpoint.GetNumberOfCheckpoint());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
