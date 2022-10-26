using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOneController : MonoBehaviour
{

    public Transform m_DetectionPointLevel;
    float m_DetectionDistance = 1f;
    void Update()
    {
        if(DetectCollision())
        {
            SceneManager.LoadSceneAsync("Level_Two");
        }
    }

    bool DetectCollision()
    {
        return (m_DetectionPointLevel.transform.position - GameController.GetGameController().GetPlayer().transform.position).magnitude <= m_DetectionDistance;
    }
}
