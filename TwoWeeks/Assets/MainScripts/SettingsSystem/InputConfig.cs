using UnityEngine;

namespace SettingsSystem
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        [SerializeField] private KeyCode Interaction_Key;
    }
}
