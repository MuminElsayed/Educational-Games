using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Shape
{
    public string name;
    public Sprite[] answerSprites;
    public Sprite questionSprite;
    public AudioClip shapeClip;
}
public class matchShapes : MonoBehaviour
{
    [SerializeField]
    private int quizID, playerScore, maxScore, numberOfPlays;
    [SerializeField]
    private Shape[] shapes;
    [SerializeField]
    private GameObject[] questions, answers;
    [SerializeField]
    private AudioClip[] startingAudio;
    [SerializeField]
    private AudioClip allCorrectClip;
    [SerializeField]
    private bool answersRSprites, questionsRSprites;
    private int correctAnswersCounter = 0, sheetplaysCounter;
    public static Action resetAnswers;
    private int[] randomNumbersArray;
    private ParticleSystem sparkles;
    [SerializeField]
    private bool shuffle = true;

    void Start()
    {
        sparkles = GameObject.FindGameObjectWithTag("Sparkles").GetComponent<ParticleSystem>();
    }

    public void newGame()
    {
        StartCoroutine(startNewGame());
    }

    IEnumerator startNewGame()
    {
        // WorksheetManager.instance.transitionOut();
        // yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0f);
        correctAnswersCounter = 0;
        List<int> numbersList = new List<int>{};
        for (int i = 0; i < shapes.Length; i++)
        {
            numbersList.Add(i);
        }

        //Randomize a list
        var rnd = new System.Random();
        var randomNumbers = numbersList.OrderBy(item => rnd.Next());
        randomNumbersArray = randomNumbers.ToArray();
        //

        int questionsCounter = 0;
        int answersCounter = 0;
        if (questionsRSprites)
        {
            foreach (var item in questions)
            {
                questions[questions.Length - questionsCounter - 1].GetComponentInChildren<DropSpace>().questionNumber = randomNumbersArray[questionsCounter];
                questions[questions.Length - questionsCounter - 1].transform.GetChild(0).GetComponent<Image>().sprite = shapes[randomNumbersArray[questionsCounter]].questionSprite;
                questionsCounter ++;
            }
        } else { //Using text instead of shapes
            foreach (var item in questions)
            {
                if (shuffle)
                {
                    questions[questions.Length - questionsCounter - 1].GetComponentInChildren<DropSpace>().questionNumber = randomNumbersArray[questionsCounter];
                    questions[questions.Length - questionsCounter - 1].transform.GetComponentInChildren<TextMeshProUGUI>().text = shapes[randomNumbersArray[questionsCounter]].name.ToUpper();
                } else {
                    questions[questionsCounter].GetComponentInChildren<DropSpace>().questionNumber = numbersList[questionsCounter];
                    questions[questionsCounter].transform.GetComponentInChildren<TextMeshProUGUI>().text = shapes[numbersList[questionsCounter]].name.ToUpper();
                }
                questionsCounter ++;
            }
        }

        if (answersRSprites)
        {
            foreach (var item in answers)
            {
                answers[answersCounter].GetComponentInChildren<DragNDrop>().answerNumber = randomNumbersArray[answersCounter];
                answers[answersCounter].GetComponentInChildren<DragNDrop>().shapeClip = shapes[randomNumbersArray[answersCounter]].shapeClip;
                answers[answersCounter].GetComponent<Image>().sprite = shapes[randomNumbersArray[answersCounter]].answerSprites[UnityEngine.Random.Range(0, shapes[randomNumbersArray[answersCounter]].answerSprites.Length)]; //Puts a random answer sprite
                answersCounter ++;
            }
        } else {
            foreach (var item in answers)
            {
                answers[answersCounter].GetComponentInChildren<DragNDrop>().answerNumber = randomNumbersArray[answersCounter];
                answers[answersCounter].GetComponentInChildren<DragNDrop>().shapeClip = shapes[randomNumbersArray[answersCounter]].shapeClip;
                answers[answersCounter].GetComponentInChildren<TextMeshProUGUI>().text = shapes[randomNumbersArray[answersCounter]].name.ToLower();
                answersCounter ++;
            }
        }
        resetAnswers();
    }

    void SubmitAnswer(bool answer)
    {
        if (answer)
        {
            correctAnswersCounter ++;
            playerScore ++;
            if (correctAnswersCounter == questions.Length)
            {
                if (sheetplaysCounter < numberOfPlays)
                {
                    WorksheetManager.instance.restartSheet();
                    newGame();
                    sheetplaysCounter++;
                } else {
                    WorksheetManager.instance.nextSheet();
                }
                sparkles.Play();
                AudioManager.instance.playAudio(allCorrectClip);
            }
        } else {
            if (playerScore > 0)
            {
                playerScore --;
            }
        }
    }


    void OnEnable() {
        DropSpace.submitAnswer += SubmitAnswer;
        newGame();
        AudioManager.instance.playAudio(startingAudio, 0.75f, 1.5f);
        // WorksheetManager.instance.transitionIn();
        if (WorksheetManager.instance != null)
        {
            WorksheetManager.instance.setMaxScore(quizID, maxScore);
        }
    }

    void OnDisable() 
    {
        DropSpace.submitAnswer -= SubmitAnswer;
        if (WorksheetManager.instance != null)
        {
            WorksheetManager.instance.postScore(quizID, playerScore);
        }
    }
}