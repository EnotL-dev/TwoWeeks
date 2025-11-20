using UnityEngine;

namespace InteractionSystem
{
    public class InteractionManager
    {
        public void ProcessInteraction(InteractableObject interactable)
        {
            if(interactable is DialogObject d_obj)
            {
                Main.MainManagers.dialogManager.StartDialog(d_obj);
            }
            else if(interactable is ItemObject i_obj)
            {
                ItemInteraction(i_obj);
            }
        }

        private void ItemInteraction(ItemObject itemObj)
        {
            itemObj.StartInteractedEvents?.Invoke();
        }
    }
}
