using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    private GameObject player;

    public Transform bulletPos;

    private Animator anim;
    private SpriteRenderer sprite;

    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        anim.SetBool("IsAttacking", false);

        if (distance < 10)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                anim.SetBool("IsAttacking", true);
            }
        }
    }

    void Shoot()
    {
        var rb = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector3 dir = sprite.flipX ? Vector3.right : Vector3.left;
        rb.velocity = 5.0f * dir;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
        }
    }
}
