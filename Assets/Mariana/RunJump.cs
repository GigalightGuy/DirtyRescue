using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunJump : MonoBehaviour
{
    [Header("For Patrol")]
    public Transform groundCheckPoint;
    public Transform wallCheckPoint;

    public float speed;
    private float moveDirection = 1;
    public float circleRadius;

    public LayerMask groundLayer;
    
    private bool facingRight = true;
    private bool checkGround;
    private bool checkWall;

    
    [Header("For Jump")]
    private Transform player;
    public Transform groundCheck;

    public float jumpHeight;

    public Vector2 boxSize;

    private bool isGrounded;


    [Header("For SeePlayer")]
    public Vector2 lineOfSite;

    public LayerMask playerLayer;

    private bool canSeePlayer;


    [Header("Other")]
    private Rigidbody2D enemyrb;

    private Animator enemyAnim;

    void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        player = Player.Instance.transform;
    }

    void FixedUpdate()
    {
        checkGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        AnimationController();
        if (!canSeePlayer && isGrounded) 
        {
            Patrol();
        }
        
    }

    void Patrol()
    {
        if (!checkGround || checkWall) 
        {
            if (facingRight)
            {
                Flip();
            }
            else if (!facingRight)
            {
                Flip();
            }
        }
        enemyrb.velocity = new Vector2(speed * moveDirection, enemyrb.velocity.y);
    }

    void JumpAttack() 
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        if (isGrounded) 
        {
            enemyrb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
    }

    void FlipToPlayer() 
    {
        float playerPosition = player.position.x - transform.position.x;
        if (playerPosition < 0 && facingRight) 
        {
            Flip();
        }
        else if (playerPosition > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip() 
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180,0);
    }

    void AnimationController() 
    {
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
        Gizmos.DrawCube(groundCheck.position, boxSize);
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    }
}
