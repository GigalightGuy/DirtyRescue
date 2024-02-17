using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMultiplierUI : MonoBehaviour
{
    private TextMeshProUGUI m_ScoreMultiplierText;

    private void Start()
    {
        m_ScoreMultiplierText = GetComponent<TextMeshProUGUI>();
        LevelManager.Instance.ScoreMultiplierChanged += OnScoreMultiplierChanged;
    }

    private void OnScoreMultiplierChanged(int value)
    {
        m_ScoreMultiplierText.text = "x" + value;
    }
}
