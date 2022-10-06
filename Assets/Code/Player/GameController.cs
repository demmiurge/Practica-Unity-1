using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    float m_PlayerLife = 1.0f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameController GetGameController()
    {
        if(m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
        }
        return m_GameController;
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

    public float GetLife()
    {
        return m_PlayerLife;
    }
}

