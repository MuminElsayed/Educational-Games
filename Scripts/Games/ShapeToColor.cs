using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ShapeToColor : MonoBehaviour
{
    [SerializeField]
    private bool effectText = true, sparkles = false;
    private Button button;
    public Color currentColor;
    private TextMeshProUGUI buttonText;
    private Color originalColor, textColor;
    private Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        textColor = buttonText.color;
        originalColor = image.color;
        //change colors
    }

    void correctAnswerEffect()
    {
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
        image.color = originalColor;
        buttonText.color = textColor;
        button.interactable = true;
    }

    void sendAnswer()
    {
        image.color = MatchShapesColors.currentColor;
        if (MatchShapesColors.submitAnswerAction(currentColor))
        {
            correctAnswerEffect();
        } else {
            StartCoroutine(wrongAnswerEffect());
        }
    }

    public void setShape(Sprite sprite, Color color, string text)
    {
        image.sprite = sprite;
        buttonText.text = text;
        currentColor = color;
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
