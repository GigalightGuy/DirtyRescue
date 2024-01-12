using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _healthBar;

    // Update is called once per frame
    void Update()
    {
        if ( _player.m_Health == 6)
        {
            _healthBar.fillAmount = 1f;
        }
        if (_player.m_Health == 5)
        {
            _healthBar.fillAmount = 0.833f;
            Debug.Log("has 5 health");
        }
        if (_player.m_Health == 4)
        {
            _healthBar.fillAmount = 0.67f;
            Debug.Log("has 4 health");
        }
        if (_player.m_Health == 3)
        {
            _healthBar.fillAmount = 0.5f;
            Debug.Log("has 3 health");
        }
        if (_player.m_Health == 2)
        {
            _healthBar.fillAmount = 0.333f;
            Debug.Log("has 2 health");
        }
        if (_player.m_Health == 1)
        {
            _healthBar.fillAmount = 0.166f;
            Debug.Log("has 1 health");
        }
        if (_player.m_Health == 0)
        {
            _healthBar.fillAmount = 0;
            Debug.Log("has 0 health");
        }
    }
}
