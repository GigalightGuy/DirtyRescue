using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPetrolOil : MonoBehaviour
{
    public bool onFire;
    public float timer;
    public bool isIn = false;

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
                    Debug.Log("Continuous vida -1");
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
            Debug.Log("vida -1");
            isIn = true;           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("has exited");
        isIn = false;
        timer = 0;
    }
}
