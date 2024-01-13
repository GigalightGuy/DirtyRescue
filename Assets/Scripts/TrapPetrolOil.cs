using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPetrolOil : MonoBehaviour
{
    [SerializeField] private bool onFire;
    [SerializeField] private float timer;
    [SerializeField] private bool isIn = false;

    private void Start()
    {
        onFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire == true)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            if (isIn)
            {
                timer += Time.deltaTime;
                if (timer > 2f)
                {
                    Player.Instance.TakeDamage(1);
                    timer = 0;
                }
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onFire == true && collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
            Debug.Log("took 1 damage");
            isIn = true;           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isIn = false;
        timer = 0;
    }
}
