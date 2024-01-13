using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (/*boss is ded &&*/ collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Clear");
        }
    }
}
