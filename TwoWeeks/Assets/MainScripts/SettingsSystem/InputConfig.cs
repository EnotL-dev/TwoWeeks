using UnityEngine;

namespace SettingsSystem
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public KeyCode Interaction_Key;
        [Space(5)]
        [Header("Dialog system")]
        public KeyCode Skip_Message_Key = KeyCode.Space;
    }
}
