using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateEnemy : MonoBehaviour
{
    public GameObject miniEnemy;
    private GameObject player;

    public Transform miniPos;

    public Animator anim;

    private float distance;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("StartAnim", false);
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 10)
        {
            anim.SetBool("StartAnim", true);
            timer += Time.deltaTime;

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
}
