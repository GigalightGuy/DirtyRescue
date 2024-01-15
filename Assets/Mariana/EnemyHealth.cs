using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public Animator anim;

    public GameObject fim;
    public GameObject fimSpawn;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0) 
        {
            anim.SetBool("IsHit", true);   
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            health -= 5; 
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Instantiate(fim, fimSpawn.transform.position, Quaternion.identity);
    }
}
