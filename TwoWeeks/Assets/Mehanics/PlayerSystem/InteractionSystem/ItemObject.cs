using UnityEngine;

namespace InteractionSystem
{
    public class ItemObject : InteractableObject
    {
        public Item item;

        public override void Interact()
        {
            Debug.Log("Item taken");
        }
    }
}
