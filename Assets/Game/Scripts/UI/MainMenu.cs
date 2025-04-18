using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject controlsMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void PlayButtonClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OptionsButtonClicked()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void ControllsButtonClicked()
    {
        optionMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void BackButtonClicked()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

}
