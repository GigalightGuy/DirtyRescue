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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween) 
        {
           
            if (player.transform.position.x > this.transform.position.x)
            {
                rb.velocity = new Vector2(speed, 0);
            }

            if (player.transform.position.x < this.transform.position.x)
            {
                rb.velocity = new Vector2(-speed, 0);
            }
        }
    }
}
