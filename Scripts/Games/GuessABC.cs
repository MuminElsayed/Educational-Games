using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Letters
{
    public char letter;
    public Sprite sprite;
    public AudioClip clip;
}
public class GuessABC : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu, gameCanvas;
    public static GuessABC instance;
    [SerializeField]
    private Letters[] allLetters;
    private AudioSource audioSource;
    public int solution;
    public int[] currentLetters;
    [SerializeField]
    private Image[] buttons;
    [SerializeField]
    private TextMeshProUGUI placeholderText;
    private int lastSolution;
    private Animator[] anims;
    private int appearHash = Animator.StringToHash("Appear");
    private int wrongHash = Animator.StringToHash("Wrong");
    private int selection;
    [SerializeField]
    private AudioClip[] positiveClip, negativeClip;
    [SerializeField]
    private AudioClip letterClip, chooseTheLetterClip;
    private string[] validations = new string[5] {"That's correct!", "Great work!", "Good job!", "Keep it up!", "Nice work!"};
    public Sprite[] fishSprites;
    private bool wrongRunning = false;
    private bool correctRunning = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anims = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            newQuestion();
        }
    }

    public void submitAnswer(int button)
    {
        selection = button;
        if (button == solution)
        {
            if (!correctRunning)
            {
                StartCoroutine("correctAnswer");
            }
        } else {
            if (!wrongRunning)
            {
                StartCoroutine("wrongAnswer");
            }
        }
    }

    void newQuestion()
    {
        currentLetters = getRandLetters();
        for (int i = 0; i < currentLetters.Length; i++)
        {
            buttons[i].sprite = allLetters[currentLetters[i]].sprite;
        }
        solution = UnityEngine.Random.Range(0, 3); //Picks a random solution
        while (currentLetters[solution] == lastSolution) //Prevents same answers in a row
        {
            solution = UnityEngine.Random.Range(0, 3);
        }
        lastSolution = currentLetters[solution];
        placeholderText.text = "'Choose the letter " + allLetters[currentLetters[solution]].letter + "'";
        switchAnimationsBool(true);
        playAudioClip(letterClip);
        playAudioClip(allLetters[currentLetters[solution]].clip); //Plays the correct letter sound
    }

    int[] getRandLetters() //Returns an array of 3 random ints
    {
        //Check for duplicates
        int[] randomArray = new int[] {1, 2, 3};
        int prevNum = randomArray[0];
        for (int i = 0; i < randomArray.Length; i++)
        {
            randomArray[i] = UnityEngine.Random.Range(0, allLetters.Length);
            while(randomArray[i] == prevNum || randomArray[2] == randomArray[0]) //Prevents duplicates between 1 & 2, 2 & 3
            {
                randomArray[i] = UnityEngine.Random.Range(0, allLetters.Length);
            }
            prevNum = randomArray[i];
        }

        if (randomArray[0] == randomArray[1] || randomArray[1] == randomArray[2] || randomArray[0] == randomArray[2])
            {
                Debug.LogError("DUPES");
            }
        return randomArray;
    }

    IEnumerator correctAnswer()
    {
        //Play correct audio here
        correctRunning = true;
        StopCoroutine("playAudioClip");
        StopCoroutine("wrongAnswer");
        wrongRunning = false;
        playAudioClip(positiveClip[UnityEngine.Random.Range(0, positiveClip.Length)]);
        placeholderText.text = getRandomValidation();
        yield return new WaitForSeconds(audioSource.clip.length);
        switchAnimationsBool(false);
        yield return new WaitForSeconds(0.5f);
        newQuestion();
        correctRunning = false;
    }

    IEnumerator wrongAnswer()
    {
        wrongRunning = true;
        anims[selection].SetTrigger(wrongHash);
        playAudioClip(negativeClip[UnityEngine.Random.Range(0, negativeClip.Length)]);
        placeholderText.text = "Try again!";
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        playAudioClip(letterClip);
        playAudioClip(allLetters[currentLetters[solution]].clip); //Plays the correct letter sound again
        placeholderText.text = "'Choose the letter " + allLetters[currentLetters[solution]].letter + "'";
        wrongRunning = false;
    }

    IEnumerator tempText()
    {
        yield return new WaitForSeconds(3f);
    }

    void switchAnimationsBool(bool value)
    {
        foreach (Animator animator in anims)
        {
            animator.SetBool(appearHash, value);
            print("anims");
        }
    }

    string getRandomValidation()
    {
        string output = validations[UnityEngine.Random.Range(0, validations.Length)];
        return output;
    }

    void playAudioClip(AudioClip clip)
    {
        StartCoroutine(playClip(clip));
    }

    IEnumerator playClip(AudioClip clip)
    {
        while(audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void startGame()
    {
        playAudioClip(chooseTheLetterClip);
        newQuestion();
        gameCanvas.SetActive(true);
        startMenu.SetActive(false);
    }
}