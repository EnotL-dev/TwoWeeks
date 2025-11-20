using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using EasyTextEffects;
using EasyTextEffects.Effects;

namespace PlayerSystem.DialogSystem
{
    [System.Serializable]
    public class MessageBlock
    {
        public bool autoskip = true;
        public bool canSkip = false;
        [Space(5)]
        public int msDelay_before_next_message = 1000; //ms
        [FormerlySerializedAs("effectsList")]
        [Space(5)]
        public List<GlobalTextEffectEntry> endEffects;
        public List<GlobalTextEffectEntry> startEffects; //speed determined by animation
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
