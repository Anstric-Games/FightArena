using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    //public void SelectStage(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName);
    //}

    public GameObject gameStages;
    private GameObject[] allStages;
    private int currentIndex = 0;
    private string sceneName;

    private void Start()
    {
        allStages = new GameObject[gameStages.transform.childCount];

        for (int i = 0; i < gameStages.transform.childCount; i++)
        {
            allStages[i] = gameStages.transform.GetChild(i).gameObject;
            allStages[i].SetActive(false);
        }

        if (PlayerPrefs.HasKey("SelectedStageIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedStageIndex");
        }

        ShowCurrentStage(currentIndex);
    }

    void ShowCurrentStage(int currentIndex)
    {
        foreach (GameObject stage in allStages)
        {
            stage.SetActive(false);
        }

        allStages[currentIndex].SetActive(true);
    }

    public void NextStage()
    {
        currentIndex = (currentIndex + 1) % allStages.Length;

        ShowCurrentStage(currentIndex);
    }

    public void PreviousStage()
    {
        currentIndex = (currentIndex - 1 + allStages.Length) % allStages.Length;

        ShowCurrentStage(currentIndex);
    }

    public void OnYesButtonClick()
    {
        PlayerPrefs.SetInt("SelectedStageIndex", currentIndex);
        PlayerPrefs.Save();

        if (currentIndex == 0)
            sceneName = "Map1";
        else if (currentIndex == 1)
            sceneName = "Map2";
        else if (currentIndex == 2)
            sceneName = "Map3";


        SceneManager.LoadScene(sceneName);
    }

}
