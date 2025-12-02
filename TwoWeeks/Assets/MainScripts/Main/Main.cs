using UnityEngine;

public static class Main
{
    public static MainController MainControllers;
    public static MainManager MainManagers;

    public static bool lockedPlayer = false; //запрещает движение игрока и камеры

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
