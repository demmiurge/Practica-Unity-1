using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    public enum OpeningForms
    {
        Distance,
        Points,
        Keys,
        Keyboard,
    }

    bool m_IsOpen;
    FPSPlayerControllerV1 m_Player;

    public Animation m_AnimationDoor;
    public AnimationClip m_AnimationNameToOpenDoor;
    public AnimationClip m_AnimationNameToCloseDoor;

    public OpeningForms m_OpeningForm;
    public OpeningForms m_ChangeWayToOpen;

    public float m_ActivationDistance = 3;
    public float m_PointsRequired = 1000;
    public string m_NameOfKeyItem;
    public int m_NumberOfKeysRequired = 1;

    public KeyCode m_KeyToOpen = KeyCode.O;
    public KeyCode m_KeyToClose = KeyCode.C;
    public KeyCode m_KeyToInteract = KeyCode.E;

    public void GateState(bool Open = true)
    {
        if (!m_AnimationDoor.isPlaying && m_IsOpen != Open)
        {
            m_AnimationDoor.Play(Open ? m_AnimationNameToOpenDoor.name : m_AnimationNameToCloseDoor.name);
            m_IsOpen = Open;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_IsOpen = false;
        //m_Player = GameObject.FindGameObjectWithTag("Player");
        //m_Player = GameController.GetGameController().GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        m_Player = GameController.GetGameController().GetPlayer();
        switch (m_OpeningForm)
        {
            case OpeningForms.Distance:
                OpenByDistance();
                break;
            case OpeningForms.Points:
                OpenByPoints();
                break;
            case OpeningForms.Keys:
                OpenByKey();
                break;
            case OpeningForms.Keyboard:
                OpenByKeyboard();
                break;
        }
    }

    void OpenByDistance()
    {
        if (InDistance()) GateState();
        else GateState(false);
    }

    void OpenByPoints()
    {
        if (PlayerManager.instance.m_Score >= 1000)
        {
            GateState(true);
        }
    }

    void OpenByKey()
    {
        if (InDistance())
        {
            Inventory l_Inventory = m_Player.GetComponent<Inventory>();

            if (l_Inventory.CheckItemExistsByNameAndQuantity(m_NameOfKeyItem, m_NumberOfKeysRequired))
            {
                l_Inventory.ConsumeItems(m_NameOfKeyItem, m_NumberOfKeysRequired);
                m_OpeningForm = m_ChangeWayToOpen;
            }
        }
    }

    void OpenByKeyboard()
    {
        if (Input.GetKeyDown(m_KeyToClose)) GateState(false);
        if (Input.GetKeyDown(m_KeyToOpen)) GateState();
        if (Input.GetKeyDown(m_KeyToInteract)) GateState(!m_IsOpen);
    }

    private bool InDistance()
    {
        return (transform.position - m_Player.transform.position).magnitude <= m_ActivationDistance;
    }
}