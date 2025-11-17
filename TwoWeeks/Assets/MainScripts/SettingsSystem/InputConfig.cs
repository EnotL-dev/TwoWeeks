using UnityEngine;

namespace SettingsSystem
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public KeyCode Interaction_Key;
    }
}
