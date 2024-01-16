using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFishnet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.Root(2);
        }
    }
}
