using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private GameObject worksheetManager, audioManager;

    public void startGame()
    {
        audioManager.SetActive(true);
        worksheetManager.SetActive(true);
        gameObject.SetActive(false);
    }

}
