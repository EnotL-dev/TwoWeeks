using UnityEngine;
using InteractionSystem;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Movement")]
        public Moving moving;
        public Jumping jumping;
        public FootStepsSound footSteps;
        [Header("Controllers")]
        public InteractionController interactionController;

        public void Awake()
        {
            Main.MainControllers.playerController = this;
        }
    }
}
