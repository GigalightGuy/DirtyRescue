using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreEvaluation : MonoBehaviour
{
    [SerializeField] private SOFloat scoreSO;
    [SerializeField] private TMP_Text textS;
    [SerializeField] private TMP_Text textA;
    [SerializeField] private TMP_Text textB;
    [SerializeField] private TMP_Text textC;

    private TMP_Text currentScoreText;

    private void Start()
    {
        textC.gameObject.SetActive(true);
        currentScoreText = textC;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreSO.Value > 1000)
        {
            currentScoreText.gameObject.SetActive(false);
            textS.gameObject.SetActive(true);
            currentScoreText = textS;

        }
        else if (scoreSO.Value > 500)
        {
            currentScoreText.gameObject.SetActive(false);
            textA.gameObject.SetActive(true);
            currentScoreText = textA;
        }
        else if (scoreSO.Value > 250)
        {
            currentScoreText.gameObject.SetActive(false);
            textB.gameObject.SetActive(true);
            currentScoreText = textB;
        }
    }
}
