using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int m_Score = 0;
    public static PlayerManager instance;
    public FPSPlayerControllerV1 player;
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text ammoText;
    [SerializeField]
    TMP_Text shieldText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Points: " + m_Score.ToString();
        ammoText.text = "Ammo: " + player.GetAmmo().ToString() + "/" + player.m_MaxAmmo.ToString();
        shieldText.text = "Shield: " + player.GetShield().ToString();
    }
}
