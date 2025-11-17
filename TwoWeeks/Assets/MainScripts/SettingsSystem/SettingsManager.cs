using UnityEngine;

namespace SettingsSystem
{
    public class SettingsManager
    {
        private InputConfig cachedInputConfig;
        public LanguageIndex languageIndex = LanguageIndex.Eng;

        //Можно потом update добавить в реальнмо времени
        public InputConfig InputConfig ()
        {
            if (cachedInputConfig == null)
            {
                InputConfig playerinputConfig = Resources.Load<InputConfig>("PlayerSettings/InputConfig");

                if (playerinputConfig)
                {
                    cachedInputConfig = playerinputConfig;
                    return playerinputConfig;
                }
                else
                {
                    Debug.LogError("<color=red>ХУЛИ НЕТУ КОНФИГА ИНПУТА В РЕСУРСАХ?</color>");
                    return null;
                }
            } 
            else
            {
                return cachedInputConfig;
            }
        }

        public void ChangeLanguageIndex(LanguageIndex newIndex)
        {
            languageIndex = newIndex;
        }
    }
}
