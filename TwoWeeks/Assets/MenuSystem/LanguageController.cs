using TMPro;
using UnityEngine;

namespace MenuSystem
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI languageTextExample;
        [SerializeField] private TextMeshProUGUI startText;

        private void Start()
        {
            Main.MainManagers.cursorManager.ShowCursor();
        }

        private void ChangeExampleText()
        {
            LanguageIndex languageIndex = Main.MainManagers.settingsManager.languageIndex;
            if (languageIndex == LanguageIndex.Eng)
            {
                languageTextExample.text = "Select language";
                startText.text = "[E] - Start";
            }
            if (languageIndex == LanguageIndex.Ru)
            {
                languageTextExample.text = "Выбрать язык";
                startText.text = "[E] - Старт";
            }
        }

        public void ChangeLanguage(int languageIndex)
        {
            Main.MainManagers.settingsManager.languageIndex = (LanguageIndex)languageIndex;
            ChangeExampleText();
        }
    }
}