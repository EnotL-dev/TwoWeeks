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

        public void OnPointerEnter(InteractableObject interactable)
        {   
            if (interactable is ItemObject i_obj)
            {
                ItemPointerEnter(i_obj);
            }
        }

        public void OnPointerExit(InteractableObject interactable)
        {
            if (interactable is ItemObject i_obj)
            {
                ItemPointerExit(i_obj);
            }
        }

        private void ItemInteraction(ItemObject itemObj)
        {
            itemObj.StartInteractedEvents?.Invoke();
        }

        private void ItemPointerEnter(ItemObject itemObj)
        {
            itemObj.OnPointerEnter?.Invoke();
        }

        private void ItemPointerExit(ItemObject itemObj)
        {
            itemObj.OnPointerExit?.Invoke();
        }
    }
}
