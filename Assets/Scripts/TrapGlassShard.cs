using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGlassShard : MonoBehaviour
{
    [SerializeField] private Player _player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.TakeDamage(1);
            Debug.Log("took 1 damage");
        }
    }
}
