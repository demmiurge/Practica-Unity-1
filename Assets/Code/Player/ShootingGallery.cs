using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootingGallery : MonoBehaviour
{
    [Space(0.5f)]
    [Header("Targets")]
    [Space(1f)]
    public List<GameObject> m_TargetList;

    [Space(0.5f)]
    [Header("HUD")]
    [Space(1f)]
    public Canvas m_PlayerHud;
    public TMP_Text m_TextMeshPro;
    public Canvas m_Message;
    public TMP_Text m_TextMessage;

    static ShootingGallery m_ShootingGallery;
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
    }

    public static ShootingGallery GetShootingGallery()
    {
        return m_ShootingGallery;
    }

    public void ActivateShootingGallery()
    {
        //Debug.Log("Shooting Gallery");
        for(int i = 0; i < m_TargetList.Count; i++)
        {
            m_TargetList[i].GetComponent<Animation>().Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_TextMeshPro.gameObject.SetActive(true);
            m_Message.gameObject.SetActive(true);
            //StartCoroutine(HideMessage());
        }
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(5);
        m_Message.gameObject.SetActive(false);
    }
}
