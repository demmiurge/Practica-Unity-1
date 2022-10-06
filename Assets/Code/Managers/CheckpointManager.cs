using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private CheckpointController[] m_Checkpoints;
    private CheckpointController m_LatestCheckpointReached;

    public Transform GetLatestCheckpointReached() => m_LatestCheckpointReached.transform;

    public bool m_ChangeCheckpoint = false;
    public bool m_AutoRespawn = false;
    public KeyCode m_LastCheckpointKeyCode = KeyCode.Keypad1;
    public KeyCode m_NextCheckpointKeyCode = KeyCode.Keypad2;
    

    // Start is called before the first frame update
    void Start()
    {
        m_Checkpoints = GameObject.FindObjectsOfType<CheckpointController>();

        m_Checkpoints = m_Checkpoints.OrderBy(l_Checkpoint => l_Checkpoint.GetNumberOfCheckpoint()).ToArray();

        m_LatestCheckpointReached = m_Checkpoints[0];
        m_LatestCheckpointReached.ActiveIsChecked();
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
        if (Input.GetKeyDown(KeyCode.R) && m_LatestCheckpointReached)
        {
            Transform l_PlayerPosition = GameObject.FindWithTag("Player").transform;
            l_PlayerPosition.position = m_LatestCheckpointReached.transform.position;
            ActivatePreviousPositions();
        }

        if (Input.GetKeyDown(m_LastCheckpointKeyCode) && m_ChangeCheckpoint) ChangeCurrentCheckpoint(-1);
        if (Input.GetKeyDown(m_NextCheckpointKeyCode) && m_ChangeCheckpoint) ChangeCurrentCheckpoint(1);
    }

    private void ChangeCurrentCheckpoint(int index)
    {
        int l_Index = m_LatestCheckpointReached.GetNumberOfCheckpoint() + index;

        if (l_Index < 0) l_Index = 0;
        if (l_Index > m_Checkpoints.Length - 1) l_Index = m_Checkpoints.Length - 1;

        if (m_Checkpoints[l_Index].GetStatusCheck()) {
            m_LatestCheckpointReached = m_Checkpoints[l_Index];

            if (m_AutoRespawn)
                FindObjectOfType<FPSPlayerControllerV1>().transform.position = m_LatestCheckpointReached.transform.position;
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
