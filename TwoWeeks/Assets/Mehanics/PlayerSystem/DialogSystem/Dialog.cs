using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/Dialog")]
    public class Dialog : ScriptableObject
    {
        [SerializeField]
        private List<KeyValueLanguage> messages;

        public string GetMessage()
        {
            LanguageIndex language = Main.MainManagers.settingsManager.languageIndex;

            return messages.Find(x => x.key == language).value;
        }
    }
}
