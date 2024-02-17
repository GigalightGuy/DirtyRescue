using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject enemy;
    private Rigidbody2D rb;

    public float force;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
