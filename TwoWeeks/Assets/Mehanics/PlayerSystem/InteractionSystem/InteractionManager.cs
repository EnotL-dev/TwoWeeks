using UnityEngine;

namespace InteractionSystem
{
    public class InteractionManager
    {
        public void ProcessInteraction(InteractableObject interactable)
        {
            interactable.Interact();
        }
    }
}
