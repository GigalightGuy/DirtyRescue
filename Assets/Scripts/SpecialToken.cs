using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialToken : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            _image.gameObject.SetActive(false);
            Scoring.Instance.AddScore(500);
        }

    }
}
