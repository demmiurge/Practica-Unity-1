using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int m_Score = 0;
    public static ScoreManager instance;
    [SerializeField]
    TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Points: " + m_Score.ToString();
    }
}
