using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    [System.Serializable]
    public class MessageBlock
    {
        public bool autoskip = true;
        public bool canSkip = true;
        [Space(5)]
        public float speed_char = 0.05f;
        public Material materialText;
        [SerializeField]
        public List<KeyValueLanguage> name_person;
        [SerializeField]
        public List<KeyValueLanguageArea> message;
    }

    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/Dialog")]
    public class Dialog : ScriptableObject
    {
        public bool lockPlayer = false;
        [SerializeField]
        private List<MessageBlock> blockMes;

        private LanguageIndex language => Main.MainManagers.settingsManager.languageIndex;

        public MessageBlock GetMessageBlock(int index)
        {
            return blockMes[index];
        }

        public int CountBlocks()
        {
            return blockMes.Count;
        }

        public string GetNamePerson(int index)
        {
            return blockMes[index].name_person.Find(x => x.key == language).value;
        }

        public string GetMessage(int index)
        {
            return blockMes[index].message.Find(x => x.key == language).value;
        }
    }
}
