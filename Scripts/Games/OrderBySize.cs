using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderBySize : MonoBehaviour
{
    public ImageClass[] shapes;
    private List<int> fixedSizes;
    [SerializeField]
    private GameObject[] answers;
    private Sprite questionSprite;
    public int correctAnswer, correctAnswerCounter;
    [SerializeField]
    private bool descendingOrder = true;
    public static Action<string> SetAnswer;

    void Start()
    {
        newGame();
    }

    void newGame()
    {
        fixedSizes = new List<int>(){500, 425, 350, 275, 200, 125}; //Fixed sizes for the 6 questions

        if (descendingOrder) //From big to small
        {
            correctAnswer = 0;
        } else { //From small to big
            correctAnswer = answers.Length - 1;
        }

        //Get random sprite for this game
        int rndNum = UnityEngine.Random.Range(0, shapes.Length);
        questionSprite = shapes[rndNum].sprite;
        AudioManager.instance.playAudio(shapes[rndNum].audioClips, 1f, 1f);

        //Gets a shuffled sizes list to randomize the order
        List<int> shuffledArray = SharedMethods.instance.ShuffleList(fixedSizes);

        for (int i = 0; i < answers.Length; i++) //Always 6 options
        {
            //Sets answer sprites properties (size, random rotation)
            answers[i].GetComponentInChildren<answerButton>().SetShape(questionSprite, shuffledArray[i], shuffledArray[i], 0);
            //Gives each sprite button the question index (0 largest - 5 smallest)
            answers[i].GetComponentInChildren<answerButton>().SetCurrentAnswer(shuffledArray[i].ToString());
        }

        if (SetAnswer != null)
            SetAnswer(fixedSizes[correctAnswer].ToString()); //not working
    }

    void CheckAnswer(string answer)
    {
        if (String.Equals(answer, fixedSizes[correctAnswer].ToString()))
        {
            if (descendingOrder)
            {
                correctAnswer ++;
            } else {
                correctAnswer --;
            }
            correctAnswerCounter ++;
        }

        if (correctAnswerCounter == answers.Length)
        {
            WorksheetManager.instance.nextSheet();
        } else {
            SetAnswer(fixedSizes[correctAnswer].ToString());
        }
    }

    void OnEnable()
    {
        answerButton.submitAnswer += CheckAnswer;
    }

    void OnDisable()
    {
        answerButton.submitAnswer -= CheckAnswer;
    }
}
