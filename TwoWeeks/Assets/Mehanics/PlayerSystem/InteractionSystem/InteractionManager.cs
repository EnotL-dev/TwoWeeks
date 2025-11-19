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
                ProccesInteract(i_obj.item);
            }
        }

        private void ProccesInteract(Item item)
        {

        }
    }
}
