using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private SOFloat scoreSO;

    private void Update()
    {
        scoreText.text = "Pontuação: " + scoreSO.Value + "";
    }
}
