using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGlassShard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("vida-1");
        }
    }
}
