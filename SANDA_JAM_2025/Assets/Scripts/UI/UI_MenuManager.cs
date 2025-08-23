using UnityEngine;

public class UI_MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject credits;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
        credits.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(false);
        credits.SetActive(true);
    }
}
