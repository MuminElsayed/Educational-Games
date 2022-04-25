using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    [SerializeField]
    private Vector3 defaultPos;
    private Image image;
    private Color imageColor;
    private float imageAlpha;
    public int answerNumber;
    public static Action<int> setAnswer;
    public AudioClip shapeClip;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        imageColor = image.color;
        imageAlpha = image.color.a;
        defaultPos = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.playAudio(shapeClip);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        imageColor.a = 0.75f;
        image.color = imageColor;
        setAnswer(answerNumber);
        //Add shape audio here
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        imageColor.a = imageAlpha;
        image.color = imageColor;
        //submit answer
    }

    public void ResetPos()
    {
        image.raycastTarget = true;
        rectTransform.anchoredPosition = defaultPos;
    }

    void OnEnable() {
        // ResetPos();
        matchShapes.resetAnswers += ResetPos;
        finishThePatterns.resetSheet += ResetPos;
    }

    private void OnDisable() {
        matchShapes.resetAnswers -= ResetPos;
        finishThePatterns.resetSheet += ResetPos;
    }
}