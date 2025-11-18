using PlayerSystem.DialogSystem;
using UnityEngine;

namespace InteractionSystem
{
    public class DialogObject : InteractableObject
    {
        public Dialog dialog;

        public override string GetHintName()
        {
            if (dialog.GetNamePerson(0) != null)
                return dialog.GetNamePerson(0);
            else
                return null;
        }
    }
}
