using UnityEngine;

namespace InteractionSystem
{
    public class ItemObject : InteractableObject
    {
        public Item item;

        public override string GetHintName()
        {
            if (item.GetName() != null)
                return item.GetName();
            else
                return null;
        }
    }
}
