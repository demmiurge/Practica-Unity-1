using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShootingGallery : MonoBehaviour
{
    public Canvas m_PlayerHud;
    public TMP_Text m_TextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateShootingGallery()
    {
        //Debug.Log("Shooting Gallery");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_TextMeshPro.gameObject.SetActive(true);
        }
    }
}
