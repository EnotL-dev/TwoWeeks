using UnityEngine;

namespace SettingsSystem
{
    public class SettingsManager
    {
        private static InputConfig cachedConfig;

        //Можно потом update добавить в реальнмо времени
        public static InputConfig InputConfig ()
        {
            if (cachedConfig == null)
            {
                InputConfig playerinputConfig = Resources.Load<InputConfig>("PlayerSettings/InputConfig");

                if (playerinputConfig)
                {
                    cachedConfig = playerinputConfig;
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
                return cachedConfig;
            }
        }
    }
}
