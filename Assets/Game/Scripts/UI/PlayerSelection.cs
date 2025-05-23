using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject playerCharacters;
    public GameObject[] allCharacters;
    private int currentIndex = 0;

    private void Start()
    {
        allCharacters = new GameObject[playerCharacters.transform.childCount];

        for (int i = 0; i < playerCharacters.transform.childCount; i++)
        {
            allCharacters[i] = playerCharacters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }

        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        }

        ShowCurrentCharacter(currentIndex);

    }

    void ShowCurrentCharacter(int currentIndex)
    {
        foreach (GameObject character in allCharacters)
        {
            character.SetActive(false);
        }

        allCharacters[currentIndex].SetActive(true);
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % allCharacters.Length;

        ShowCurrentCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + allCharacters.Length) % allCharacters.Length;

        ShowCurrentCharacter(currentIndex);
    }

    public void OnYesButtonCLick(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }



}
