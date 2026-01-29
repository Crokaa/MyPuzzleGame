
public enum UIName
{
    // Texts

    ContinueText,
    QuitToMenuText,
    SettingsText,
    StartGameText,
    ChooseLevelText,
    QuitGameText,
    BackText,
    InteractText,

    // Scenes

    MenuScene,
    TestLevelScene,

    // Tags

    MenuCanvasTag

}

public static class UINamesHelper
{
    public static string GetName(UIName name)
    {
        return name.ToString();
    }
}
