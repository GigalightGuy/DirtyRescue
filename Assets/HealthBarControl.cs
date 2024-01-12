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
        int playerHealth = _player.Health;
        if (playerHealth == 6)
        {
            _healthBar.fillAmount = 1f;
        }
        if (playerHealth == 5)
        {
            _healthBar.fillAmount = 0.833f;
            Debug.Log("has 5 health");
        }
        if (playerHealth == 4)
        {
            _healthBar.fillAmount = 0.67f;
            Debug.Log("has 4 health");
        }
        if (playerHealth == 3)
        {
            _healthBar.fillAmount = 0.5f;
            Debug.Log("has 3 health");
        }
        if (playerHealth == 2)
        {
            _healthBar.fillAmount = 0.333f;
            Debug.Log("has 2 health");
        }
        if (playerHealth == 1)
        {
            _healthBar.fillAmount = 0.166f;
            Debug.Log("has 1 health");
        }
        if (playerHealth == 0)
        {
            _healthBar.fillAmount = 0;
            Debug.Log("has 0 health");
        }
    }
}
