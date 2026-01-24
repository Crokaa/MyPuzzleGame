using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetMenuCanvas();
    }

    private void SetMenuCanvas()
    {
        GameObject menuCanvas = GameObject.FindGameObjectWithTag(UINamesHelper.GetName(UIName.MenuCanvasTag));

        Button[] menuButtons = menuCanvas.GetComponentsInChildren<Button>();
        foreach (Button button in menuButtons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINamesHelper.GetName(UIName.StartGameText))
                button.onClick.AddListener(() =>
                {
                    // This will be changed but for now it works
                    GameManager.instance.StartGame();

                });
            else if (button.name == UINamesHelper.GetName(UIName.ChooseLevelText))
                button.onClick.AddListener(() => Debug.Log("Choose Level"));
            else if (button.name == UINamesHelper.GetName(UIName.SettingsText))
                button.onClick.AddListener(() => Debug.Log("Settings"));
            else if (button.name == UINamesHelper.GetName(UIName.QuitGameText))
                button.onClick.AddListener(() => Debug.Log("Quitting the game..."));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
