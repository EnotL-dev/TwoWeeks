

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    [System.Serializable]
    public class CanvasForPerson
    {
        [Header("Person")]
        public string person_tag = "1";
        public Transform person_transform;
        [Space(5)]
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI textStartPerson;
        public TextMeshProUGUI textEndPerson;
    }
}
