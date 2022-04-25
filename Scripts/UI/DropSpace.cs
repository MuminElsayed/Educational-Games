using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DropSpace : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
    public int questionNumber;
    private int selectedAnswer;
    public static Action<bool> submitAnswer;
    private GameObject lastDragged;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void checkAnswer()
    {
        if (questionNumber == selectedAnswer)
        {
            //correct answer
            submitAnswer(true);
            AudioManager.instance.playCorrectAnswer();
            StartCoroutine(disableRaycast(lastDragged.GetComponent<Image>()));
        } else {
            //wrong answer
            submitAnswer(false);
            AudioManager.instance.playWrongAnswer();
        }
    }

    private IEnumerator disableRaycast(Image image)
    {
        yield return new WaitForEndOfFrame();
        image.raycastTarget = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // print("dropped");
            eventData.pointerDrag.GetComponent<RectTransform>().position = rectTransform.position;
            lastDragged = eventData.pointerDrag;
            checkAnswer();
        } else {
            print("pointer drag null");
        }
    }

    void changeSelectedAnswer(int answer)
    {
        selectedAnswer = answer;
    }

    void OnEnable()
    {
        DragNDrop.setAnswer += changeSelectedAnswer;
    }

    void OnDisable() 
    {
        DragNDrop.setAnswer += changeSelectedAnswer;
        StopAllCoroutines();
    }
}
