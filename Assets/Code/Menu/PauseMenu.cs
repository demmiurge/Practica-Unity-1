using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject menuOptions;
    public GameObject menuSettings;

    float _time;
    bool _pause;

    private void OnEnable()
    {
        //Player.OnPause += Pause;
    }

    private void OnDisable()
    {
        //Player.OnPause += Pause;
    }

    private void Start()
    {
        menuPause.GetComponent<Animator>().SetBool("Pause", false);
        menuSettings.SetActive(false);
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
        //GameObject.FindObjectOfType<Player>().Pause();
    }

    public void Pause(bool ispaused)
    {
        _pause = ispaused;
        if (ispaused)
        {
            _time = 0.0f;
            menuPause.GetComponent<Animator>().SetBool("Pause", true);
            //MusicPlayer.Instance.PlayFX("Buttons_menu_SelectOption");
        }
        else
        {
            //OptionsClose();
            menuPause.GetComponent<Animator>().SetBool("Pause", false);
            Time.timeScale = 1f;
        }
    }

    public void ExitClick()
    {
        //SceneManager.LoadScene("MainMenu");
        //MusicPlayer.Instance.PlayFX("Buttons_menu_Back");
    }
}
