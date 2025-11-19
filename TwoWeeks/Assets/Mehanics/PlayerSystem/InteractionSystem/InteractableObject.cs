using UnityEngine;

namespace InteractionSystem
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public bool ForcedCall = false;
        public abstract string GetHintName();
    }
}
