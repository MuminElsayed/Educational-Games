using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class colorImporter : MonoBehaviour
{
    private int counter;
    [SerializeField]
    private GameObject allColors, targetColorsClass;
    private ColorClass[] colors;

    void OnEnable()
    {
        importColors(allColors);
    }

    private void importColors(GameObject allColorsObj)
    {
        //Takes all colors/names in game components and puts them in a colorClass
        Transform[] allColors = allColorsObj.GetComponentsInChildren<Transform>();
        colors = new ColorClass[allColors.Length - 1];
        // counter = 0;
        foreach (Transform color in allColors)
        {
            if (color.childCount == 0)
            {
                colors[counter] = new ColorClass();
                colors[counter].color = color.GetComponent<Image>().color;
                colors[counter].name = color.gameObject.name;
                counter ++;
            }
        }
        targetColorsClass.GetComponent<Drawing>().colors = colors;
    }
}
