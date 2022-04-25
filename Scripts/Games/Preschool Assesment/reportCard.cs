using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class reportCard : MonoBehaviour
{
    [SerializeField]
    private GameObject[] scoreResults;

    void OnEnable()
    {
        for (int i = 0; i < scoreResults.Length; i++)
        {
            float scorePercent = WorksheetManager.instance.getScore(i);
            string textOutput = "";
            if (scorePercent > 0.8f)
            {
                textOutput = "Excellent!";
            } else if (scorePercent > 0.6f)
            {
                textOutput = "Very Good!";
            } else {
                textOutput = "Good!";
            }
            scoreResults[i].GetComponent<TextMeshProUGUI>().text = textOutput;
        }
    }
}
