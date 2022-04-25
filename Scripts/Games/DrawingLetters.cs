using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


[System.Serializable]
public class ColorClass
{
    public string name;
    public Color color;
}
public class DrawingLetters : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
{
    [SerializeField]
    private GameObject trailPrefab;
    [SerializeField]
    private int numberOfPages = 7;
    private Camera cam;
    private bool drawing = false, correctDraw = false;
    private GameObject currentTrail;
    private int currentColorID, trailCounter, correctLetter, correctColor, pageMaxScore, playerScore, counter, currentPage, lastLetter, lastColor;
    private float playerCummulativeScore, gameMaxScore;
    [SerializeField]
    private ColorClass[] colors;
    private List<GameObject> trails = new List<GameObject>();
    [SerializeField]
    private GameObject drawings, lettersHolder, cursor, startMenu, playingMenu, endgameMenu;
    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    [SerializeField]
    private TextMeshProUGUI questionText, scoreText, gradeTitle, scoreDetails;
    private Image cursorImage;
    private List<GameObject> correctAnswers = new List<GameObject>();
    [SerializeField]
    private ParticleSystem sparkles;
    [SerializeField]
    private AudioSource sparkleAudioSrc;
    private AudioSource audioSrc1;
    [SerializeField]
    private AudioClip tryAgain, wrongColor, wrongLetter, questionAudio;
    [SerializeField]
    private AudioClip[] letterAudio, colorAudio;
    [SerializeField]
    private AudioClip[] correctAnswer;
    [SerializeField]
    private Animator gameMenuAnim;
    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        Cursor.visible = false;
        cursorImage = cursor.GetComponent<Image>();
        startMenu.SetActive(true);
        playingMenu.SetActive(false);
        endgameMenu.SetActive(false);
        audioSrc1 = GetComponent<AudioSource>();
        audioSrc1.PlayDelayed(1.5f); //Plays title delayed
    }

    void Update()
    {
        //Sets cursor position
        Vector3 cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(cursorPos.x, cursorPos.y, cursor.transform.position.z);
    }

    public void startGame()
    {
        startMenu.SetActive(false);
        endgameMenu.SetActive(false);
        getLetters();
        gameMenuAnim.Play("GameMenuStart");
        playingMenu.SetActive(true);
    }

    public void getLetters()
    {
        changeColor(-1);
        //Gets a random color and letter as the answer
        //Prevents duplicate questions
        while (lastLetter == correctLetter)
        {
            correctLetter = UnityEngine.Random.Range(0, alphabet.Length);
        }
        while (lastColor == correctColor)
        {
            correctColor = UnityEngine.Random.Range(0, colors.Length);   
        }
        //Saves last answer
        lastColor = correctColor;
        lastLetter = correctLetter;
        //Plays question audio
        StartCoroutine(playQuestionAudio());
        //Sets question text to chosen letter/color
        questionText.text = "Color the letters " + alphabet[correctLetter] + " " + colors[correctColor].name + ".";
        //Randomizes letters on board
        TextMeshProUGUI[] shownLetters = lettersHolder.GetComponentsInChildren<TextMeshProUGUI>();
        // holders = new holder[shownLetters.Length];
        counter = 0;
        foreach (TextMeshProUGUI letter in shownLetters)
        {
            // holders[counter].letter = letterScript.currentLetter; //Assigns text to holder class
            char randomLetter = alphabet[UnityEngine.Random.Range(0, alphabet.Length)]; //Gets a random letter
            // holders[counter].text.text = randomLetter.ToString(); //Assigns letters to the placeholder class
            letter.text = randomLetter.ToString(); //Assigns letters to the placeholders
            counter ++;
        }
        //Assigns the correct letter to 5 random ones on screen
        for (int i = 0; i < 4; i++)
        {
            shownLetters[UnityEngine.Random.Range(0, shownLetters.Length)].text = alphabet[correctLetter].ToString();
        }
        foreach (TextMeshProUGUI letter in shownLetters)
        {
            if (letter.text == alphabet[correctLetter].ToString())
            {
                pageMaxScore ++;
            }
        }
        gameMaxScore += pageMaxScore;
        scoreText.text = "0/" + pageMaxScore;
        //1-Adds correct answers to list, to prevent duplicate answers
        //2-get max score by looping through all letters
        //3- on correct answer, check if already answered in the list
        //4- if not, add player score
    }

    public void goNextPage()
    {
        StopAllCoroutines();
        StartCoroutine(nextPage());
    }

    public IEnumerator nextPage()
    {
        //Gets all trails and deactivates
        playerCummulativeScore += playerScore;
        Transform[] alltrails = drawings.GetComponentsInChildren<Transform>();
        for (int i = 1; i < alltrails.Length; i++)
        {
            alltrails[i].gameObject.SetActive(false);
        }
        if (currentPage < numberOfPages)
        {
            gameMenuAnim.Play("GameScreenEnd");
            yield return new WaitForSeconds(1f);
            pageMaxScore = 0;
            playerScore = 0;
            currentPage ++;
            correctAnswers = new List<GameObject>();
            getLetters();
            gameMenuAnim.Play("GameScreenStart");
        } else { //Last page
            gameMenuAnim.Play("GameScreenEnd");
            yield return new WaitForSeconds(0.5f);
            endScreen();
        }
    }

    void endScreen()
    {
        startMenu.SetActive(false);
        playingMenu.SetActive(false);
        endgameMenu.SetActive(true);
        getGrade();
    }

    public void getGrade()
    {
        float grade = playerCummulativeScore/gameMaxScore;
        scoreDetails.text = playerCummulativeScore.ToString() + "/" + gameMaxScore.ToString();
        gradeTitle.text = "Well done!";
        playAudioClip(correctAnswer[UnityEngine.Random.Range(0, correctAnswer.Length)], 0);
    }

    public void Undo() //Deactivates the most recent trail
    {
        if (trailCounter > 0)
        {
            trails[trailCounter - 1].SetActive(false);
            trailCounter --;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //If can draw, creates a trail
    {
        
        // print(eventData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().text); //Gets the text in the object clicked on (the letter)
        if (eventData.pointerEnter.tag == "DrawingBoard" && !drawing && currentColorID > -1) //If pointer is on drawing background (can draw)
        {
            //Creates trail and follows mouse
            drawing = true;
            currentTrail = Instantiate(trailPrefab, Vector3.zero, Quaternion.identity, drawings.transform);
            currentTrail.GetComponent<TrailRenderer>().startColor = colors[currentColorID].color;
            currentTrail.GetComponent<TrailRenderer>().endColor = colors[currentColorID].color;
            trails.Add(currentTrail);
            trailCounter = trails.Count;
            checkAnswer(eventData);
        }
    }

    void checkAnswer(PointerEventData eventData)
    {
        //Checks if on the correct letter and color
        //Gets the text in the object clicked on and compares strings (less efficient but simpler implementation)
        try
        {
            if (eventData.pointerEnter.GetComponentInChildren<TextMeshProUGUI>().text == alphabet[correctLetter].ToString())
            {
                if (currentColorID == correctColor)
                {
                    if (!correctAnswers.Contains(eventData.pointerEnter)) //First time getting this gameObject correctly
                    {
                        correctAnswers.Add(eventData.pointerEnter); //Adds to the answered objects
                        correctDraw = true;
                    }
                } else {
                    // print ("Wrong color");
                    playAudioClip(wrongColor, 0.5f);
                }
            } else if (currentColorID == correctColor){
                // print("Wrong letter");
                playAudioClip(wrongLetter, 0.5f);
            } else {
                // print("Try again");
                playAudioClip(tryAgain, 0.5f);
            }   
        } catch {
            
        }
    }

    public void OnDrag(PointerEventData eventData) //Trail follows mouse
    {
        if (drawing)
        {
            Vector2 clickPos = cam.ScreenToWorldPoint(eventData.position); //Gets mouse position on screen
            currentTrail.transform.position = new Vector3(clickPos.x, clickPos.y, currentTrail.transform.position.z); //Trail follows the mouse
        }
    }

    public void OnEndDrag(PointerEventData eventData) //Stopped trail
    {
        drawing = false;
        if (correctDraw)
        {
            if (playerScore < pageMaxScore) //Adds to player score
            {
            playerScore ++;
            }
            if (playerScore == pageMaxScore)
            {
            //Plays finished anim and audio
            sparkles.Play();
            sparkleAudioSrc.Play();
            Invoke("goNextPage", 1f);
            }
            scoreText.text = playerScore + "/" + pageMaxScore;
            // print("Correct answer");
            playAudioClip(correctAnswer[UnityEngine.Random.Range(0, correctAnswer.Length)], 0);
            correctDraw = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.visible = false;
    }

    public void changeColor(int colorCode) //Called on each color button
    {
        if (colorCode == -1)
        {
            currentColorID = -1;
            cursorImage.color = Color.white;
        } else {
            currentColorID = colorCode;
            cursorImage.color = colors[colorCode].color;
        }
    }

    IEnumerator playQuestionAudio()
    {
        playAudioClip(questionAudio, 0.5f);
        yield return new WaitForSeconds(questionAudio.length + 0.5f);
        playAudioClip(letterAudio[correctLetter], 0f);
        yield return new WaitForSeconds(letterAudio[correctLetter].length + 0.5f);
        playAudioClip(colorAudio[correctColor], 0f);
    }

    private void playAudioClip(AudioClip audioClip, float delay)
    {
        audioSrc1.clip = audioClip;
        audioSrc1.PlayDelayed(delay);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
