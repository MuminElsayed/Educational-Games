using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] canvases;

    public void goToCanvas(int activeCanvas)
    {
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }
        canvases[activeCanvas].SetActive(true);
    }
}
