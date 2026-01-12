using PlayerSystem.DialogSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    [System.Serializable]
    public class EmoteEvents
    {
        public string tagEvent;
        public UnityEvent eventEmote;
    }

    public class DialogObject : InteractableObject
    {
        public Dialog dialog;
        [Space(5)]
        [SerializeField] public List<EmoteEvents> emoteEvents = new List<EmoteEvents>();
        [Space(5)]
        public UnityEvent DialogStartedEvents;
        [Space(5)]
        public UnityEvent DialogCompletedEvents;
        [Space(5)]
        public List<CanvasForPerson> canvases;

        public override string GetHintName()
        {
            if (dialog.GetNamePerson(0) != null)
                return dialog.GetNamePerson(0);
            else
                return null;
        }
    }
}
