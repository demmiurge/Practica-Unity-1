using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int m_Score = 0;
    public static PlayerManager instance;
    FPSPlayerControllerV1 player;
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

        timerText.outlineWidth = 0.3f;
        timerText.outlineColor = Color.black;

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + m_Score.ToString();
        ammoText.text = "Ammo: " + GameController.GetGameController().GetPlayer().GetAmmo().ToString() + "/" + GameController.GetGameController().GetPlayer().m_MaxAmmo.ToString();
        shieldText.text = "Shield: " + GameController.GetGameController().GetPlayer().GetShield().ToString();
        lifeText.text = "Life: " + GameController.GetGameController().GetPlayer().GetLife().ToString();
        timerText.text = "Timer: ";

        if(scoreText.IsActive())
        {
            scoreText.outlineWidth = 0.3f;
            scoreText.outlineColor = Color.black;
        }
    }

}
