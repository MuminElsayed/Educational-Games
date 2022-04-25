using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class colorChangerButton : MonoBehaviour
{
    public Color color;
    public TextMeshProUGUI text;
    public Image image;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
    }

    public void setVisuals(string targetText, Sprite targetSprite, Color targetColor)
    {
        text.text = targetText;
        image.sprite = targetSprite;
        image.color = targetColor;
        color = targetColor;
    }

    public void changeMouseColor()
    {
        MatchShapesColors.changeColorAction(color);
    }
}
