using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private CheckpointController[] m_Checkpoints;
    private CheckpointController m_LatestCheckpointReached;

    // Start is called before the first frame update
    void Start()
    {
        m_Checkpoints = GameObject.FindObjectsOfType<CheckpointController>();

        m_Checkpoints = m_Checkpoints.OrderBy(l_Checkpoint => l_Checkpoint.GetNumberOfCheckpoint()).ToArray();

        m_LatestCheckpointReached = m_Checkpoints[0];
        m_LatestCheckpointReached.ActiveIsChecked();

        //foreach (CheckpointController l_Checkpoint in m_Checkpoints)
        //{
        //    print("Checkpoint: " + l_Checkpoint.GetNumberOfCheckpoint());
        //}
    }

    private void OnEnable()
    {
        CheckpointController.m_PlayerReachedToCheckpoint += ActivateCheckpoint;
    }

    private void OnDisable()
    {
        CheckpointController.m_PlayerReachedToCheckpoint -= ActivateCheckpoint;
    }

    private void ActivateCheckpoint(CheckpointController l_checkpointcontroller)
    {
        m_LatestCheckpointReached = l_checkpointcontroller;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && m_LatestCheckpointReached)
        {
            Transform l_PlayerPosition = GameObject.FindWithTag("Player").transform;
            l_PlayerPosition.position = m_LatestCheckpointReached.transform.position;
            ActivatePreviousPositions();
        }
    }

    private void ActivatePreviousPositions() // Maybe need refact
    {
        for (int i = 0; i < m_LatestCheckpointReached.GetNumberOfCheckpoint(); i++)
        {
            m_Checkpoints[i].ActiveIsChecked();
        }
    }
}
