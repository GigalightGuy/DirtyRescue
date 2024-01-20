using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    void Start()
    {
        //health = maxHealth;
    }

    void Update()
    {
        //healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cigar"))
        {
            //health -= 17;
            ////Destroy(gameObject);
            Debug.Log("Perde vida");
            EnemyHealth.Instance.TakeDamage(5);
        }
    }
}
