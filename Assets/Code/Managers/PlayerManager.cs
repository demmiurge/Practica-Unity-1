using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int m_Score;
    public static PlayerManager instance;
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text ammoText;
    [SerializeField]
    TMP_Text shieldText;
    [SerializeField]
    TMP_Text lifeText;
    [SerializeField]
    TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ammoText.outlineWidth = 0.3f;
        ammoText.outlineColor = Color.black;

        shieldText.outlineWidth = 0.3f;
        shieldText.outlineColor = Color.black;

        lifeText.outlineWidth = 0.3f;
        lifeText.outlineColor = Color.black;

       
    }

    // Update is called once per frame
    void Update()
    {
        FPSPlayerControllerV1 l_Player = GameController.GetGameController().GetPlayer();

        scoreText.text = "Score: " + m_Score;
        ammoText.text = "Ammo: " + l_Player.GetAmmo() + "/" + l_Player.m_MaxAmmo;
        shieldText.text = "Shield: " + l_Player.GetShield();
        lifeText.text = "Life: " + l_Player.GetLife();
        timerText.text = "Timer: " +  l_Player.GetTime().ToString("0.0");

        if(scoreText.IsActive() && timerText.IsActive())
        {
            scoreText.outlineWidth = 0.3f;
            scoreText.outlineColor = Color.black;

            timerText.outlineWidth = 0.3f;
            timerText.outlineColor = Color.black;
        }
    }

}
