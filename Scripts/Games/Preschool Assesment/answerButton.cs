using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class answerButton : MonoBehaviour
{
    [SerializeField]
    private string correctAnswer, currentAnswer;
    public static Action<string> submitAnswer;
    private Button button;
    private TextMeshProUGUI text;
    private Image imageSprite;
    private Color frameDefaultColor;
    [SerializeField]
    private Image imageFrame;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        imageSprite = GetComponent<Image>();
        button = GetComponent<Button>();
        frameDefaultColor = imageFrame.color;
    }

    public void SetShape(Sprite shape)
    {
        imageSprite.sprite = shape;
    }

    public void SetShape(Sprite shape, int width, int height, int YRot)
    {
        imageSprite.sprite = shape;
        imageSprite.rectTransform.sizeDelta = new Vector2(width, height);
        imageSprite.rectTransform.localRotation = Quaternion.Euler(0, 0, YRot);
    }

    public void SetCurrentAnswer(string answer)
    {
        currentAnswer = answer;
    }
    
    public void SetCorrectAnswer(string answer)
    {
        correctAnswer = answer;
        if (text != null)
            text.text = correctAnswer;
    }

    public void SendAnswer()
    {
        if (string.Equals(currentAnswer, correctAnswer) == true)
        {
            imageFrame.color = Color.green;
            AudioManager.instance.playCorrectAnswer();
            button.interactable = false;
        } else {
            StartCoroutine(TurnFrameRed());
            AudioManager.instance.playWrongAnswer();
        }
        submitAnswer(currentAnswer);
    }

    IEnumerator TurnFrameRed()
    {
        frameDefaultColor = imageFrame.color;
        imageFrame.color = Color.red;
        yield return new WaitForSeconds(1f);
        imageFrame.color = frameDefaultColor;
    }

    void OnEnable()
    {
        chooseByVoiceGrid.setQuestion += SetCurrentAnswer;
        OrderBySize.SetAnswer += SetCorrectAnswer;
    }
    void OnDisable()
    {
        chooseByVoiceGrid.setQuestion -= SetCurrentAnswer;
        OrderBySize.SetAnswer -= SetCorrectAnswer;
    }
}
