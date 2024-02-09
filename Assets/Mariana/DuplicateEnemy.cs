using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateEnemy : MonoBehaviour
{
    public GameObject miniEnemy;
    private GameObject player;

    public Transform miniPos;

    private Animator anim;

    private float distance;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        anim.enabled = false;

        if (distance < 10)
        {
            timer += Time.deltaTime;
            anim.enabled = true;

            if (timer > 2)
            {
                timer = 0;
            }
        }
    }

    void Duplicate() 
    {
        Instantiate(miniEnemy, miniPos.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
        }
    }
}
