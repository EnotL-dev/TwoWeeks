using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class ItemObject : InteractableObject
    {
        public Item item;
        [Space(5)]
        public UnityEvent StartInteractedEvents;

        public override string GetHintName()
        {
            if (item.GetName() != null)
                return item.GetName();
            else
                return null;
        }
    }
}
