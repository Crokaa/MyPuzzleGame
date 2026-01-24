using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    void Awake()
    {
        SetOnPauseButtons();
    }

    private void SetOnPauseButtons()
    {

        GameObject inGameCanvas = GameObject.FindGameObjectWithTag(UINamesHelper.GetName(UIName.InGameCanvasTag));

        Button[] buttons = inGameCanvas.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINamesHelper.GetName(UIName.ContinueText))
                button.onClick.AddListener(() => GameManager.instance.PauseUnpause());
            else if (button.name == UINamesHelper.GetName(UIName.SettingsText))
                button.onClick.AddListener(() => Debug.Log("Settings"));
            else if (button.name == UINamesHelper.GetName(UIName.QuitToMenuText))
                button.onClick.AddListener(() => GameManager.instance.GoToMenu());
        }
    }
}
