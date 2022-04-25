using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel, libraryPanel;
    public void selectBook(int bookNum)
    {
        
    }

    public void goToLibrary()
    {
        libraryPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void goToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        libraryPanel.SetActive(false);
    }
}
