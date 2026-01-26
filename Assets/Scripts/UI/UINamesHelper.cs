
public enum UIName
{
    ContinueText,
    QuitToMenuText,
    SettingsText,
    MenuScene,
    TestLevelScene,
    MenuCanvasTag,
    StartGameText,
    ChooseLevelText,
    QuitGameText,
    InGameCanvasTag,
    SettingsCanvasTag
}

public static class UINamesHelper
{
    public static string GetName(UIName name)
    {
        return name.ToString();
    }
}
