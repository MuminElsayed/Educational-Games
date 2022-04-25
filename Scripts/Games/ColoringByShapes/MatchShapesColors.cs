using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;


[System.Serializable]
public class questionHolder
{
    public Sprite sprite;
    public string shapeName;
    public Color correctColor;
    public colorChangerButton questionFrame;
}

public class MatchShapesColors : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] startingAudio;
    public static Color currentColor;
    [SerializeField]
    private Color[] allColors;
    private List<Color> usedColors;
    [SerializeField]
    private GameObject[] shapeHolders;
    [SerializeField]
    private questionHolder[] questions;
    public static Func<Color, bool> submitAnswerAction;
    public static Action<Color> changeColorAction;
    private int correctAnswerCounter;

    void Start()
    {
        Invoke("newGame", 0.1f);
    }

    void newGame()
    {
        if (startingAudio != null)
        {
            AudioManager.instance.playAudio(startingAudio, 1f, 1f);
        }
        usedColors = new List<Color>();
        //Sets a random correct color for each shape
        foreach (questionHolder question in questions)
        {
            int randomNum = UnityEngine.Random.Range(0, allColors.Length);
            while (usedColors.Contains(allColors[randomNum]))
            {
                randomNum = UnityEngine.Random.Range(0, allColors.Length);
            }
            question.correctColor = allColors[randomNum];
            usedColors.Add(allColors[randomNum]);
        }

        //Sets the shape questions
        foreach (GameObject item in shapeHolders)
        {
            int randomNum = UnityEngine.Random.Range(0, questions.Length);
            item.GetComponent<ShapeToColor>().setShape(questions[randomNum].sprite, questions[randomNum].correctColor, questions[randomNum].shapeName);

            questions[randomNum].questionFrame.setVisuals(questions[randomNum].shapeName + " =", questions[randomNum].sprite, questions[randomNum].correctColor);
        }
    }

    public bool _submitAnswer(Color answer)
    {
        bool returnAnswer;
        if (currentColor == answer)
        {
            correctAnswerCounter++;
            returnAnswer = true;
        } else {
            returnAnswer = false;
        }

        if (correctAnswerCounter == shapeHolders.Length)
        {
            WorksheetManager.instance.nextSheet();
        }
        return returnAnswer;
    }

    public void changeColor(Color targetColor)
    {
        currentColor = targetColor;
        cursor.instance.changeCursorColor(targetColor);
    }
    
    void OnEnable()
    {
        submitAnswerAction += _submitAnswer;
        changeColorAction += changeColor;
    }  
    
    void OnDisable()
    {
        submitAnswerAction -= _submitAnswer;
        changeColorAction -= changeColor;
    }
}
