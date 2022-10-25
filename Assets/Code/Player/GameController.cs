using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    float m_PlayerLife = 1.0f;
    FPSPlayerControllerV1 m_Player;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameController GetGameController()
    {
        if(m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
            GameControllerData l_GameControllerData = Resources.Load<GameControllerData>("GameControllerData");
            m_GameController.m_PlayerLife = l_GameControllerData.m_Life;
        }
        return m_GameController;
    }

    public FPSPlayerControllerV1 GetPlayer()
    {
        return m_Player;
    }

    public void SetPlayer(FPSPlayerControllerV1 Player)
    {
        m_Player = Player;
    }

    public static void DestroySingleton()
    {
        if(m_GameController != null)
            GameObject.Destroy(m_GameController.gameObject);
        m_GameController = null;
    }

    public void SetLife(float PlayerLife)
    {
        m_PlayerLife = PlayerLife;
    }

    public float GetPlayerLife()
    {
        return m_PlayerLife;
    }

    public void RestartGame()
    {
        m_Player.RestartGame();
    }
}

