using PlayerSystem.DialogSystem;
using UnityEngine;

namespace InteractionSystem
{
    public class DialogObject : InteractableObject
    {
        public Dialog dialog;
        public override void Interact()
        {
            Debug.Log("Dialog opened");
        }
    }
}
