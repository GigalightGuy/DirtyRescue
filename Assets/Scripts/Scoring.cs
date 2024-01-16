using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Scoring : MonoBehaviour
{
    private static Scoring _scoring;
    public static Scoring Instance => _scoring;

    private TMP_Text _text;
    [SerializeField] private int _score = 0;

    private void Awake()
    {
        if (_scoring)
        {
            Destroy(gameObject);
            return;
        }
        _scoring = this;
    }
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_text != null)
        {
            _text.text = "Pontuação: " + _score;
        }
    }

    public void AddScore(int score)
    {
        _score += score;
    }
}
