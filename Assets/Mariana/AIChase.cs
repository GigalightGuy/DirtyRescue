using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    private float distance;
    private Rigidbody2D rb;
    public Animator anim;

    public string facing = "right";
    public string previousFacing;

    private void Awake()
    {
        previousFacing = facing;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();   
    }

    void Update()
    {
        anim.SetBool("IsRunning", false);

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween) 
        {
            if (player.transform.position.x > this.transform.position.x)
            {
                rb.velocity = new Vector2(speed, 0);
                anim.SetBool("IsRunning", true);
                facing = "left";
            }

            if (player.transform.position.x < this.transform.position.x)
            {
                rb.velocity = new Vector2(-speed, 0);
                anim.SetBool("IsRunning", true);
                facing = "right";
            }
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        DetermineFacing(move);
    }

    void DetermineFacing(Vector2 move)
    {
        if (previousFacing != facing)
        {
            previousFacing = facing;
            gameObject.transform.Rotate(0, 180, 0);
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
