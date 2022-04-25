using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ButtonListener : MonoBehaviour
{
    [SerializeField]
    private bool effectText = true, changeColor = true, sparkles = false;
    public UnityEvent targetFunction;
    private Button button;
    public string answer;
    private TextMeshProUGUI buttonText;
    private Color borderColor, textColor;
    private Image imageBorder;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        imageBorder = GetComponent<Image>();
        textColor = buttonText.color;
        borderColor = imageBorder.color;
        //change colors
    }

    void correctAnswerEffect()
    {
        if (changeColor)
        {
            imageBorder.color = Color.green;
        }

        if (effectText)
        {
            buttonText.color = Color.green;
        }

        if (sparkles)
        {
            AudioManager.instance.playCorrectAnswer();
        }

        button.interactable = false;
    }

    IEnumerator wrongAnswerEffect()
    {
        imageBorder.color = Color.red;

        if (effectText)
        {
            buttonText.color = Color.red;
        }

        AudioManager.instance.playWrongAnswer();
        yield return new WaitForSeconds(1f);
        resetEffects();
    }

    void resetEffects()
    {
        imageBorder.color = borderColor;
        buttonText.color = textColor;
        button.interactable = true;
    }

    void sendAnswer()
    {
        if (Rhyming.submitAnswerAction(answer))
        {
            // print("correct");/
            //change text/border to green
            correctAnswerEffect();
        } else {
            // print("false");
            StartCoroutine(wrongAnswerEffect());
            //change text/border to red
        }
        targetFunction.Invoke();
    }

    public void changeText(string text)
    {
        buttonText.text = text;
        answer = text;
    }

    void OnEnable()
    {
        Rhyming.resetButtons += resetEffects;
        button.onClick.AddListener(() => sendAnswer());
    }

    void OnDisable()
    {
        Rhyming.resetButtons -= resetEffects;
        button.onClick.RemoveAllListeners();
        resetEffects();
    }
}