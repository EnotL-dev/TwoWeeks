using UnityEngine;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        public Moving moving;
        public Jumping jumping;
        public FootStepsSound footSteps;

        public void Awake()
        {
            Main.mainController.playerController = this;
        }
    }
}
