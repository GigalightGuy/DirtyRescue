using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClear : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (/*boss is ded &&*/ collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
