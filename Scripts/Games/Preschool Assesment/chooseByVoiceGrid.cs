using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class chooseByVoiceGrid : MonoBehaviour
{
    [SerializeField]
    private int quizID, playerScore, maxScore;
    [SerializeField]
    private AudioClip[] startingAudio;
    [SerializeField]
    private GameObject lettersPanel;
    private int[] randomNumbersArray;
    [SerializeField]
    private int questionNum;
    [SerializeField]
    private GameObject[] letterHolders;
    [SerializeField]
    private AudioClip[] lettersAudio;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private bool shapes = false;
    [SerializeField]
    private string[] theAlphabet;
    private string question;
    [SerializeField]
    private List<int> previousQuestions;
    public static Action<string> setQuestion;
    [SerializeField]
    private bool shuffle = true;
    

    void Start()
    {
        newGame();
    }

    void newGame()
    {
        StartCoroutine(startNewGame());
        questionNum = -1;
    }

    IEnumerator startNewGame()
    {
        randomizeLetters();
        yield return new WaitForSeconds(2f);
        newQuestion();
    }

    void randomizeLetters()
    {
        previousQuestions = new List<int>();
        var rnd = new System.Random();
        int[] numbersArray = new int[letterHolders.Length];
        for (int i = 0; i < letterHolders.Length; i++)
        {
            numbersArray[i] = i;
        }
        var randomNumbers = numbersArray.OrderBy(item => rnd.Next());
        randomNumbersArray = randomNumbers.ToArray();

        for (int i = 0; i < letterHolders.Length; i++)
        {
            letterHolders[i].GetComponentInChildren<answerButton>().SetCurrentAnswer(theAlphabet[randomNumbersArray[i]].ToString());
            if (shapes)
            {
                letterHolders[i].GetComponentInChildren<answerButton>().SetShape(sprites[randomNumbersArray[i]]);
            }
        }
    }

    void checkAnswer(string answer)
    {
        if (string.Equals(answer, question))
        {
            // print("right");
            previousQuestions.Add(questionNum);
            playerScore ++;
            newQuestion();
        } else {
            // print("wrong");
            playCurrentQuestion(1.5f);
            if (playerScore > 0)
            {
                playerScore --;
            }
        }
    }

    void newQuestion()
    {
        if (previousQuestions.Count == theAlphabet.Length)
        {
            WorksheetManager.instance.nextSheet();
        } else {
            if (shuffle)
            {
                questionNum = UnityEngine.Random.Range(0, theAlphabet.Length);
            } else {
                questionNum ++;
            }
            question = theAlphabet[questionNum].ToString();
            if (previousQuestions.Contains(questionNum)){
                //Restart loop if identical question found
                newQuestion();
                return;
            } else {
                //Set the unique question
                setQuestion(question);
            }
            //Play question audio here
            AudioManager.instance.playAudio(lettersAudio[questionNum], 1.5f);
        }
    }

    public void playCurrentQuestion(float delay)
    {
        AudioManager.instance.playAudio(lettersAudio[questionNum], delay);
    }

    void OnEnable()
    {
        answerButton.submitAnswer += checkAnswer;
        AudioManager.instance.playAudio(startingAudio, 1f, 1.5f);
        if (WorksheetManager.instance != null)
        {
            WorksheetManager.instance.setMaxScore(quizID, maxScore);
        }
    }

    void OnDisable()
    {
        answerButton.submitAnswer -= checkAnswer;
        if (WorksheetManager.instance != null)
        {
            WorksheetManager.instance.postScore(quizID, playerScore);
        }
    }
}
