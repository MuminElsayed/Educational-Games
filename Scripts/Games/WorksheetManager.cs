using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScoreSheet
{
    public string quizName;
    public int maxScore, playerScore;
}
public class WorksheetManager : MonoBehaviour
{
    public static WorksheetManager instance;
    [SerializeField]
    private GameObject[] workSheets;
    [SerializeField]
    private ScoreSheet[] scoreSheet;
    [SerializeField]
    private GameObject startScreen, endScreen;
    public int sheetCounter = 0;
    private Animator worksheetAnimator;
    [SerializeField]
    private AudioClip[] startingAudio;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        worksheetAnimator = GetComponent<Animator>();
        foreach (GameObject item in workSheets)
        {
            item.SetActive(false);
        }
        endScreen.SetActive(false);
        startScreen.SetActive(true);
        if (startingAudio != null && AudioManager.instance != null)
        {
            AudioManager.instance.playAudio(startingAudio, 0.75f, 1f);
        }
    }

    public void postScore(int quizID, int scoreValue)
    {
        if (scoreSheet.Length != 0)
        {
            scoreSheet[quizID].playerScore += scoreValue;
        }
    }

    public void setMaxScore(int quizID, int maxScoreValue)
    {
        if (scoreSheet.Length != 0)
        {
            scoreSheet[quizID].maxScore += maxScoreValue;
        }
    }

    public float getScore(int quizID)
    {
        return (float)scoreSheet[quizID].playerScore/(float)scoreSheet[quizID].maxScore;
    }

    public void startGame()
    {
        StartCoroutine("startGameEnum");
    }

    IEnumerator startGameEnum()
    {
        transitionOut();
        yield return new WaitForSeconds(1f);
        workSheets[0].SetActive(true);
        startScreen.SetActive(false);
        transitionIn();
    }

    public void transitionIn()
    {
        worksheetAnimator.SetTrigger("GoIn");
    }

    public void transitionOut()
    {
        worksheetAnimator.SetTrigger("GoOut");
    }

    public void nextSheet()
    {
        StartCoroutine(nextSheetEnum());
    }

    public void restartSheet()
    {
        StartCoroutine(restartSheetEnum());
    }

    IEnumerator nextSheetEnum()
    {
        transitionOut();
        yield return new WaitForSeconds(1f);
        workSheets[sheetCounter].SetActive(false);
        sheetCounter++;
        if (sheetCounter < workSheets.Length)
        {
            workSheets[sheetCounter].SetActive(true);
        } else {
            endScreen.SetActive(true);
        }
        // yield return new WaitForSeconds(0.5f);
        transitionIn();
    }

    IEnumerator restartSheetEnum()
    {
        transitionOut();
        yield return new WaitForSeconds(1f);
        transitionIn();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}