using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGlassShard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
            Debug.Log("took 1 damage");
        }
    }
}
