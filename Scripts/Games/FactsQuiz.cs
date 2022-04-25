using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FactsQuiz : MonoBehaviour
{
    [System.Serializable]
    public class Facts
    {
        [TextArea(2,4)]
        public string text1, text2;
        // public AudioClip audio;
        public Sprite image1, image2;
    }
    [System.Serializable]
    public class Questions    
    {
        [TextArea(1,2)]
        public string text;
        // public AudioClip audio;
        [TextArea(2,3)]
        public string[] answers = new string[] {"True", "False"};
        public int correctAnswer;
    }
    [SerializeField]
    private Facts[] facts;
    [SerializeField]
    private Image image1, image2;
    private int currentFacts = 0;
    [SerializeField]
    private TextMeshProUGUI textbox1, textbox2;
    [SerializeField]
    private GameObject backButton, nextButton, restartButton, gamePanel;


    void Start()
    {
        getFacts();
    }

    void getFacts()
    {
        textbox1.text = facts[currentFacts].text1;
        textbox2.text = facts[currentFacts].text2;
        image1.sprite = facts[currentFacts].image1;
        image2.sprite = facts[currentFacts].image2;
    }

    void getQuestions()
    {
        
    }

    public void next()
    {
        if (currentFacts < facts.Length - 1)
        {
            currentFacts ++;
            getFacts();
        } 
        if (currentFacts == facts.Length - 1)
        {
            nextButton.SetActive(false);
            restartButton.SetActive(true);
        }
        
        backButton.SetActive(true);
    }

    public void previous()
    {
        if (currentFacts > 0)
        {
            currentFacts --;
            getFacts();
        } else { //First fact page
            gamePanel.SetActive(true);
            gameObject.SetActive(false);
        }
        nextButton.SetActive(true);
        restartButton.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
