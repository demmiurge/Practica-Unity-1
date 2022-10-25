using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    public float m_Health;
    public float m_MaxHealth;

    public GameObject m_HealthBarUI;
    public Slider m_Slider;

    // Start is called before the first frame update
    void Start()
    {
        m_Health = m_MaxHealth;
        m_Slider.value = CalculateHealth();
        m_HealthBarUI.SetActive(false);
    }

    // To only have to work between 0 and 1 is more optimal for unity
    float CalculateHealth()
    {
        return m_Health / m_MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        m_Slider.value = CalculateHealth();

        if (m_Health < m_MaxHealth)
            m_HealthBarUI.SetActive(true);
        
        if (m_Health <= 0)
            Destroy(gameObject);

        if (m_Health > m_MaxHealth)
            m_Health = m_MaxHealth;
    }
}
