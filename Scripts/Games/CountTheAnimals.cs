using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class CountTheAnimals : MonoBehaviour
{
    [SerializeField]
    private GameObject[] patterns;
    [SerializeField]
    private ImageClass[] allAnimals;
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TextMeshProUGUI[] answersText;
    private int correctAnswer, previousAnswer = 0, correctAnimal, previousAnimal = -1, correctButtonNumber, questionCounter;
    [SerializeField]
    private AudioClip titleClip, chooseYourDifficulty, howManyClip, countTheAnimalsClip, wrongAnswerClip, easyClip, hardClip;
    [SerializeField]
    private AudioClip[] correctAnswerClips;
    private AudioSource audioSource, sparklesAudioSrc;
    private Animator animator;
    [SerializeField]
    private Animator[] buttonAnims;
    private bool easyMode = false;
    [SerializeField]
    private GameObject startPanel, gamePanel, endGamePanel, patternsHolderEASY, patternsHolderHARD, sparkles;
    private List<Image> imageHoldersHARD;
    private ParticleSystem sparklesPS;

    void Start()
    {
        imageHoldersHARD = new List<Image>(patternsHolderHARD.GetComponentsInChildren<Image>());
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sparklesPS = sparkles.GetComponent<ParticleSystem>();
        sparklesAudioSrc = sparkles.GetComponent<AudioSource>();

        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(false);

        AudioManager.instance.playAudio(titleClip, 0.5f);
        AudioManager.instance.playAudio(chooseYourDifficulty, 3f);
    }

    IEnumerator getNewQuestion()
    {
        yield return new WaitForSeconds(1f);
        if (questionCounter < 3)
        {
            questionCounter++;
            animator.SetTrigger("New Question");
            yield return new WaitForSeconds(0.5f);
            if (easyMode)
            {
                getQuestionEASY();
            } else {
                getQuestionHARD();
            }
            yield return new WaitForSeconds(1f);  
            if (allAnimals[correctAnimal].audioClips != null)
            {
                AudioManager.instance.playAudio(howManyClip);
                AudioManager.instance.playAudio(allAnimals[correctAnimal].audioClips, 1f, 1f);
            } else {
                AudioManager.instance.playAudio(countTheAnimalsClip, 0f);
            }
        } else {
            //Set end screen
            animator.SetTrigger("gameEnd");
            yield return new WaitForSeconds(1f);
            gamePanel.SetActive(false);
            startPanel.SetActive(false);
            endGamePanel.SetActive(true);
            AudioManager.instance.playAudio(correctAnswerClips[UnityEngine.Random.Range(0, correctAnswerClips.Length)]);
        }
    }

    void randomizeQuestion()
    {
        //Prevent duplicate answers/questions
        while (previousAnimal == correctAnimal)
            correctAnimal = UnityEngine.Random.Range(0, allAnimals.Length);
        previousAnimal = correctAnimal;
        while(correctAnswer == previousAnswer)
            correctAnswer = UnityEngine.Random.Range(1, patterns.Length + 1);
        previousAnswer = correctAnswer;
        questionText.text = "How many " + allAnimals[correctAnimal].name + "?";
    }

    void randomizeAnswer(int maxNumber)
    {
        //Makes 3 random unique numbers
        int[] randomNumbers = {0, 0, 0};
        while (randomNumbers[0] == randomNumbers[1] || randomNumbers[0] == randomNumbers[2] || randomNumbers[1] == randomNumbers[2] || randomNumbers[0] == correctAnswer || randomNumbers[1] == correctAnswer || randomNumbers[2] == correctAnswer)
        {
            for (int i = 0; i < 3; i++)
            {
                randomNumbers[i] = UnityEngine.Random.Range(0, maxNumber + 1);
            }
        }
        //Puts number in choices
        for (int i = 0; i < 3; i++)
        {
            answersText[i].text = randomNumbers[i].ToString();
        }
        //Put correct answer in a random choice
        correctButtonNumber = UnityEngine.Random.Range(0, 3);
        answersText[correctButtonNumber].text = correctAnswer.ToString();

        //Add question audio clip

    }

    private void getQuestionEASY()
    {
        randomizeQuestion();
        //Reset patterns
        foreach (GameObject pattern in patterns)
        {
            pattern.SetActive(false);
        }
        //Sets correct animal in chosen pattern
        Image[] questionImages = patterns[correctAnswer - 1].GetComponentsInChildren<Image>();
        for (int i = 0; i < questionImages.Length; i++)
        {
            questionImages[i].sprite = allAnimals[correctAnimal].sprite;
            //Puts random size
            questionImages[i].rectTransform.sizeDelta = new Vector2(UnityEngine.Random.Range( allAnimals[correctAnimal].spriteScale.x - 50,  allAnimals[correctAnimal].spriteScale.x + 50), UnityEngine.Random.Range( allAnimals[correctAnimal].spriteScale.y - 50,  allAnimals[correctAnimal].spriteScale.y + 50));
            //Add random flip
            questionImages[i].rectTransform.localScale = Vector3.one + (Vector3.left * 2 * UnityEngine.Random.Range(0, 2));
        }
        patterns[correctAnswer - 1].SetActive(true);

        randomizeAnswer(10);
    }

    void getQuestionHARD()
    {
        randomizeQuestion();
        randomizeAnswer(20); //Put random numbers up to inserted number
        //Put random wrong animals in images
        foreach (Image image in imageHoldersHARD)
        {
            int randomAnimal = UnityEngine.Random.Range(0, allAnimals.Length);
            while (randomAnimal == correctAnimal)
                randomAnimal = UnityEngine.Random.Range(0, allAnimals.Length);
            image.sprite = allAnimals[randomAnimal].sprite;
            image.rectTransform.sizeDelta = allAnimals[randomAnimal].spriteScale;
            image.rectTransform.localScale = Vector3.one + (Vector3.left * 2 * UnityEngine.Random.Range(0, 2));
        }
        //Puts the correct number of animals in images
        for (int i = 0; i < correctAnswer; i++)
        {
            imageHoldersHARD[i].sprite = allAnimals[correctAnimal].sprite;
            imageHoldersHARD[i].rectTransform.sizeDelta = allAnimals[correctAnimal].spriteScale;
        }
        imageHoldersHARD = SharedMethods.instance.ShuffleList(imageHoldersHARD); //Shuffle the images
        patternsHolderHARD.SetActive(true);
    }

    public void submitAnswer(int buttonNumber)
    {
        if (buttonNumber == correctButtonNumber)
        {
            answeredCorrectly();
            buttonAnims[buttonNumber].SetTrigger("correctAnswer");
            playSparkles();
        } else {
            wrongAnswer();
            buttonAnims[buttonNumber].SetTrigger("wrongAnswer");
        }
    }

    void playSparkles()
    {
        sparkles.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sparklesPS.Play();
        sparklesAudioSrc.Play();
    }

    void answeredCorrectly()
    {
        StopAllCoroutines();
        AudioManager.instance.playCorrectAnswer();
        StartCoroutine(getNewQuestion());
    }

    void wrongAnswer()
    {   
        StopAllCoroutines();
        AudioManager.instance.playWrongAnswer();
    }

    public void easyModeButton()
    {
        StartCoroutine(setEasyMode());
        animator.SetTrigger("choseDifficulty");
    }

    public void hardModeButton()
    {
        StartCoroutine(setHardMode());
        animator.SetTrigger("choseDifficulty");
    }

    IEnumerator setEasyMode()
    {
        AudioManager.instance.playAudio(easyClip);
        easyMode = true;
        yield return new WaitForSeconds(1f);
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        patternsHolderEASY.SetActive(true);
        patternsHolderHARD.SetActive(false);
        // StartCoroutine(getNewQuestion());
        getQuestionEASY();
        animator.SetTrigger("gameStart");
        yield return new WaitForSeconds(1f);  
        if (allAnimals[correctAnimal].audioClips != null)
        {
            AudioManager.instance.playAudio(howManyClip);
            AudioManager.instance.playAudio(allAnimals[correctAnimal].audioClips, 1f, 1f);
        } else {
            AudioManager.instance.playAudio(countTheAnimalsClip, 0f);
        }
    }

    IEnumerator setHardMode()
    {
        AudioManager.instance.playAudio(hardClip);
        easyMode = false;
        yield return new WaitForSeconds(1f);
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        patternsHolderEASY.SetActive(false);
        patternsHolderHARD.SetActive(true);
        // StartCoroutine(getNewQuestion());
        getQuestionHARD();
        animator.SetTrigger("gameStart");
        yield return new WaitForSeconds(1f);  
        if (allAnimals[correctAnimal].audioClips != null)
        {
            AudioManager.instance.playAudio(howManyClip);
            AudioManager.instance.playAudio(allAnimals[correctAnimal].audioClips, 1f, 1f);
        } else {
            AudioManager.instance.playAudio(countTheAnimalsClip, 0f);
        }
    }

    void playAudioold(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator playClipwtf(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.playAudio(clip);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}