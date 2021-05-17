using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] List<GameObject> mainMenuItems;

    [SerializeField] GameObject campaingScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject creditsScreen;

    public void ShowCampaingScreen()
    {
        ToggleMainMenuItems(false);
        campaingScreen.SetActive(true);
    }

    public void ShowSettingsScreen()
    {
        ToggleMainMenuItems(false);
        settingsScreen.SetActive(true);
    }

    public void ShowCreditsScreen()
    {
        ToggleMainMenuItems(false);
        creditsScreen.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowMainMenuItems()
    {
        ToggleMainMenuItems(true);
    }

    private void ToggleMainMenuItems(bool show)
    {
        for (int i = 0; i < mainMenuItems.Count; i++)
        {
            mainMenuItems[i].SetActive(show);
        }

        if (campaingScreen != null)
            campaingScreen.SetActive(false);

        if (settingsScreen != null)
            settingsScreen.SetActive(false);

        if (creditsScreen != null)
            creditsScreen.SetActive(false);
    }
}
