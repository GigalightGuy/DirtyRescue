using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDuplicateEnemy : MonoBehaviour
{
    private GameObject player;
    
    private Rigidbody2D rb;

    public float speed;
    private float timer;

    public string facing = "right";
    public string previousFacing;

    private void Awake()
    {
        previousFacing = facing;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player.transform.position.x > this.transform.position.x)
        {
            rb.velocity = new Vector2(speed, 0);
            facing = "left";
        }

        if (player.transform.position.x < this.transform.position.x)
        {
            rb.velocity = new Vector2(-speed, 0);
            facing = "right";
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        DetermineFacing(move);

        timer += Time.deltaTime;

        if (timer > 4)
        {
            Destroy(gameObject);
        }
    }

    void DetermineFacing(Vector2 move)
    {
        if (previousFacing != facing)
        {
            previousFacing = facing;
            gameObject.transform.Rotate(0, 180, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
