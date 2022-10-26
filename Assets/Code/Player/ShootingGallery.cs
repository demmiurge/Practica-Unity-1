using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootingGallery : MonoBehaviour
{
    public Transform m_DetectionPoint;
    private float m_DetectionDistance = 20;

    [Space(0.5f)]
    [Header("Targets")]
    [Space(1f)]
    public List<GameObject> m_TargetList;

    [Space(0.5f)]
    [Header("HUD")]
    [Space(1f)]
    public Canvas m_PlayerHud;
    public TMP_Text m_Score;
    public TMP_Text m_TimerText;
    public Canvas m_Message;
    public TMP_Text m_TextMessage;
    public Canvas m_Aim;

    static ShootingGallery m_ShootingGallery;
    private bool m_Entered = false;
    // Start is called before the first frame update
    void Start()
    {
        m_ShootingGallery = GetComponent<ShootingGallery>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Message.isActiveAndEnabled)
        {
            //m_TextMessage.outlineColor = new Color32(255, 145, 0, 255);
            m_TextMessage.outlineColor = Color.black;
            m_TextMessage.outlineWidth = 0.4f;
        }

        if(DetectCollision() && !m_Entered)
        {
            m_Entered = true;

            if(m_Entered)
            {
                ShowMessage();
            }
        }
    }

    public static ShootingGallery GetShootingGallery()
    {
        return m_ShootingGallery;
    }

    void ShowMessage()
    {
        m_Score.gameObject.SetActive(true);
        m_TimerText.gameObject.SetActive(true);
        m_Message.gameObject.SetActive(true);
        m_Aim.gameObject.SetActive(false);

    }

    void TimeUp()
    {
        if(GameController.GetGameController().GetPlayer().GetTime() <= 0)
        {
            Debug.Log("Adeu");
            for (int i = 0; i < m_TargetList.Count; i++)
            {
                m_TargetList[i].GetComponent<Animation>().Stop();
            }
        }
    }

    public void ActivateShootingGallery()
    {
        //Debug.Log("Shooting Gallery");
        m_Message.gameObject.SetActive(false);
        m_Aim.gameObject.SetActive(true);
        for (int i = 0; i < m_TargetList.Count; i++)
        {
            m_TargetList[i].GetComponent<Animation>().Play();
        }
    }


    bool DetectCollision()
    {
        return (m_DetectionPoint.transform.position - GameController.GetGameController().GetPlayer().transform.position).magnitude <= m_DetectionDistance;
    }

}
