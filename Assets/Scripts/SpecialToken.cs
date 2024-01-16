using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialToken : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            Scoring.Instance.AddScore(500);
        }

    }
}
