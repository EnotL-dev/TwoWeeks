using UnityEngine;
using SettingsSystem;
using InteractionSystem;
using PlayerSystem.DialogSystem;

public class MainManager
{
    public CursorManager cursorManager = new CursorManager();
    public SettingsManager settingsManager = new SettingsManager();
    public InteractionManager interactionManager = new InteractionManager();
    public DialogManager dialogManager = new DialogManager();
}
