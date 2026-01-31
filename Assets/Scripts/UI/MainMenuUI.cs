using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] GameObject _menuOptions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetMenuCanvas();
    }

    private void SetMenuCanvas()
    {
        Button[] menuButtons = _menuOptions.GetComponentsInChildren<Button>();
        foreach (Button button in menuButtons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINamesHelper.GetName(UIName.StartGameText))
                button.onClick.AddListener(() =>
                {
                    // This will be changed but for now it works
                    GameManager.instance.StartGame();
                    GetComponent<Canvas>().sortingOrder = -1;

                });
            else if (button.name == UINamesHelper.GetName(UIName.ChooseLevelText))
                button.onClick.AddListener(() => Debug.Log("Choose Level"));
            else if (button.name == UINamesHelper.GetName(UIName.SettingsText))
                button.onClick.AddListener(() => GameManager.instance.OpenSettings());
            else if (button.name == UINamesHelper.GetName(UIName.QuitGameText))
                button.onClick.AddListener(() => Application.Quit());
        }
    }

    public void ShowMenuButtons()
    {
        _menuOptions.SetActive(true);
    }

    public void HideMenuButtons()
    {
        _menuOptions.SetActive(false);
    }
}
