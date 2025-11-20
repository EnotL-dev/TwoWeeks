using UnityEngine;
using InteractionSystem;
using PlayerSystem.DialogSystem;
using Unity.Cinemachine;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Camera")]
        public CinemachineInputAxisController cinemachineInputAxisController;
        [Header("Player Movement")]
        public Moving moving;
        public Jumping jumping;
        public FootStepsSound footSteps;
        [Header("Controllers")]
        public InteractionController interactionController;
        public DialogController dialogController;

        public void Awake()
        {
            Main.MainControllers.playerController = this;
        }
    }
}
