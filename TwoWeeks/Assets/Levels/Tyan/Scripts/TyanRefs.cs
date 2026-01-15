using UnityEngine;

namespace Tyan
{
    public class TyanRefs : MonoBehaviour
    {
        public AnimationSwitcher Animation => _animation;
        public EmotionSwitcher Emotion => _emotion;
        public TyanIkController IkController => _ikControl;
        public static TyanRefs Instance;

        [SerializeField] private AnimationSwitcher _animation;
        [SerializeField] private EmotionSwitcher _emotion;
        [SerializeField] private TyanIkController _ikControl;

        private void Awake()
        {
            Instance = this;
        }
    }
}