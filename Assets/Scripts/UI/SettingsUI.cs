using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    void Awake()
    {
        SetSettingsButtons();
    }

    private void SetSettingsButtons()
    {

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINamesHelper.GetName(UIName.BackText))
                button.onClick.AddListener(() => GameManager.instance.CloseSettings());
        }
    }
}
