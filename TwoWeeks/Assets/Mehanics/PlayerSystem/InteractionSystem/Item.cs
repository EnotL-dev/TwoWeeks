using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private List<KeyValueLanguage> name_item;
        [SerializeField]
        private List<KeyValueLanguage> description_item;

        public string GetName()
        {
            LanguageIndex language = Main.MainManagers.settingsManager.languageIndex;

            return name_item.Find(x => x.key == language).value;
        }

        public string GetDescription()
        {
            LanguageIndex language = Main.MainManagers.settingsManager.languageIndex;

            return description_item.Find(x => x.key == language).value;
        }
    }
}