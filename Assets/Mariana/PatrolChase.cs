using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PatrolChase : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private GameObject player;

    private Rigidbody2D rb;

    private Transform currentPoint;

    public Animator anim;

    public float speed;
    public float distanceBetween;
    private float distance;
    private float posYplayer, posYenemy, posTotalY, posXplayer, posXenemy, posTotalX;

    public bool patrolState = true;

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
        currentPoint = pointB.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //distance = Vector2.Distance(transform.position, player.transform.position);
        posXplayer = player.transform.position.x;
        posXenemy = this.transform.position.x;

        posYplayer = player.transform.position.y;
        posYenemy = this.transform.position.y;

        posTotalX = Mathf.Abs(posXplayer - posXenemy);
        posTotalY = Mathf.Abs(posYplayer - posYenemy);

        if (posTotalY < 1 && posTotalX < 8) 
        {
            Chase();
        }

        //if (distance < distanceBetween)
        //{
        //    Chase();
        //}
        else if (patrolState == true)
        {
            anim.SetBool("IsRunning", false);
            Patrol();
        }
        else 
        {
            BackPatrol();
        }
    }

    void Patrol() 
    {
        //Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0f);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            facing = "left";
            //Flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            facing = "right";
            //Flip();
            currentPoint = pointB.transform;
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        DetermineFacing(move);
    }

    void Chase()
    {
        //anim.SetBool("IsRunning", false);

        //distance = Vector2.Distance(transform.position, player.transform.position);

        //if (distance < distanceBetween)
        //{

        //    if (player.transform.position.x > this.transform.position.x)
        //    {
        //        rb.velocity = new Vector2(speed * 2, 0f);
        //        anim.SetBool("IsRunning", true);
        //        facing = "left";
        //    }

        //    if (player.transform.position.x < this.transform.position.x)
        //    {
        //        rb.velocity = new Vector2(-speed * 2, 0f);
        //        anim.SetBool("IsRunning", true);
        //        facing = "right";
        //    }
        //}

        patrolState = false;

        if (player.transform.position.x > this.transform.position.x)
        {
            anim.SetBool("IsRunning", true);
            facing = "right";
            rb.velocity = new Vector2(speed * 2, 0f);
            currentPoint = pointB.transform;
        }

        if (player.transform.position.x < this.transform.position.x)
        {
            anim.SetBool("IsRunning", true);
            facing = "left";
            rb.velocity = new Vector2(-speed * 2, 0f);
            currentPoint = pointA.transform;
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        DetermineFacing(move);
    }

    void BackPatrol()
    {
        if (Vector2.Distance(this.transform.position, pointA.transform.position) < Vector2.Distance(this.transform.position, pointB.transform.position))
        {
            facing = "right";
            currentPoint = pointB.transform;
        }
        else 
        {
            facing = "left";
            currentPoint = pointA.transform;
        }

        patrolState = true;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void DetermineFacing(Vector2 move)
    {
        if (previousFacing != facing)
        {
            previousFacing = facing;
            gameObject.transform.Rotate(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
        }
    }
}
