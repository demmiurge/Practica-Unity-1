using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_PanelBackground;
    public GameObject m_MenuOptions;
    public GameObject m_HUD;
    public GameObject m_Aim;

    static PauseMenu m_PauseMenu = null;

    float _time;
    bool _pause;

    public static PauseMenu GetPauseMenu()
    {
        if (m_PauseMenu == null)
        {
            m_PauseMenu = new GameObject("Pause").AddComponent<PauseMenu>();
        }
        return m_PauseMenu;
    }

    private void OnEnable()
    {
        FPSPlayerControllerV1.OnPause += Pause;
    }

    private void OnDisable()
    {
        FPSPlayerControllerV1.OnPause += Pause;
    }

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        m_MenuOptions.SetActive(false);
        Time.timeScale = 1f;
        _time = 0.0f;
    }

    void Update()
    {
        if (!_pause) return;

        if (_time >= 1.1f)
        {
            Time.timeScale = 0f;
        }
        _time += Time.deltaTime;

    }

    public void Continue()
    {
        GameObject.FindObjectOfType<FPSPlayerControllerV1>().Pause();
    }

    public void Pause(bool ispaused)
    {
        _pause = ispaused;
        if (ispaused)
        {
            _time = 0.0f;
            m_PanelBackground.SetActive(true);
            m_PanelBackground.GetComponent<Animation>().Play("Menu_background");
            m_MenuOptions.SetActive(true);
            m_Aim.SetActive(false);
            m_HUD.SetActive(false);
            //MusicPlayer.Instance.PlayFX("Buttons_menu_SelectOption");
        }
        else
        {
            m_MenuOptions.SetActive(false);
            m_Aim.SetActive(true);
            m_HUD.SetActive(true);
            m_PanelBackground.GetComponent<Animation>().Play("Menu_back_out");
            Time.timeScale = 1f;
        }
    }

    public void OnRestartClick()
    {
        m_PanelBackground.GetComponent<Animation>().Play("Menu_back_out");
        m_MenuOptions.SetActive(false);
        m_PanelBackground.SetActive(false);
        SceneManager.LoadScene("Level_One");
    }

    public void OnExitClick()
    {
        m_PanelBackground.GetComponent<Animation>().Play("Menu_back_out");
        m_MenuOptions.SetActive(false);
        SceneManager.LoadSceneAsync("Menu");
        //MusicPlayer.Instance.PlayFX("Buttons_menu_Back");
    }
}
