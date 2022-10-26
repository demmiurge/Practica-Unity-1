using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;


public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private GameObject m_OptionsPanel;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Changeanim", 0.0f, 6.0f);
    }

    public void OnClickNewGame()
    {
        //MusicPlayer.Instance.PlayFX("Buttons_menu_SelectOption");
        SceneManager.LoadSceneAsync("Level_One");
    }

    public void OnClickExit()
    {
        //Application.Quit(0);
        EditorApplication.ExitPlaymode();
    }
}
