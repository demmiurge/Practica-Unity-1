using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public GameObject m_PanelBackground;
    public GameObject m_MenuOptions;
    public GameObject m_PlayerHUD;
    public GameObject m_Aim;

    float _time;
    bool _dead;


    private void OnEnable()
    {
        FPSPlayerControllerV1.OnDie += Die;
    }

    private void OnDisable()
    {
        FPSPlayerControllerV1.OnDie -= Die;
    }

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        Time.timeScale = 1f;
        _time = 0.0f;
        _dead = false;
    }

    void Update()
    {
        if (!_dead) return;

        if (_time >= 1.1f)
        {
            Time.timeScale = 0f;
        }
        _time += Time.deltaTime;

    }

    public void Die(bool isdead)
    {
        _dead = isdead;
        if (isdead)
        {
            _time = 0.0f;
            m_PanelBackground.SetActive(true);
            m_PanelBackground.GetComponent<Animation>().Play("Menu_background");
            m_MenuOptions.SetActive(true);
            m_Aim.SetActive(false);
            m_PlayerHUD.SetActive(false);
            //MusicPlayer.Instance.PlayFX("Buttons_menu_SelectOption");
        }
        else
        {
            m_MenuOptions.SetActive(false);
            m_Aim.SetActive(true);
            m_PlayerHUD.SetActive(true);
            m_PanelBackground.GetComponent<Animation>().Play("Menu_back_out");
            Time.timeScale = 1f;
        }
    }

    public void OnRestartClick()
    {
        m_PanelBackground.GetComponent<Animation>().Play("Menu_back_out");
        m_MenuOptions.SetActive(false);
        m_PanelBackground.SetActive(false);
        m_PlayerHUD.SetActive(true);
        _dead = false;
        GameController.GetGameController().GetPlayer().m_Paused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        GameController.GetGameController().RestartGame();
    }

    public void OnExitClick()
    {
        m_MenuOptions.SetActive(false);
        SceneManager.LoadSceneAsync("Menu");
    }
}
