using UnityEngine;

public static class Main
{
    public static MainController MainControllers;
    public static MainManager MainManagers;

    public static bool PlayerBusy = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("Main initialized");

        MainControllers = new MainController();
        MainManagers = new MainManager();

        DefaultSettingsInitialize();
    }

    private static void DefaultSettingsInitialize()
    {
        MainManagers.cursorManager.HideCursor();
    }
}
