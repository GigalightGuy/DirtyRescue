using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private Rigidbody2D rb;

    public float force;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("EnemyShooting");

        if (enemy.GetComponent<SpriteRenderer>().flipX == false) 
        {
            Vector3 direction = - transform.position;
            rb.velocity = new Vector2(direction.x, 0).normalized * force;

            //float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (enemy.GetComponent<SpriteRenderer>().flipX == true)
        {
            Vector3 direction = transform.position;
            rb.velocity = new Vector2(direction.x, 0).normalized * force;

            //float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

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
