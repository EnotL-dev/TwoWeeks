using UnityEngine;

namespace Tyan
{
    public class AnimationSwitcher : MonoBehaviour
    {
        [SerializeField] private string _photoPosesParameterName = "PhotoPose";
        [SerializeField] private string _poseSelectorParameterName = "PoseSelector";
        [SerializeField] private Animator _animator;
        [SerializeField] private int _poseCount = 3;
        [SerializeField] private bool _debug = false;
        private int _photoPosesParameterNameId;
        private int _poseSelectorParameterNameId;

        public void CallPhotoPose(bool selector)
        {
            _animator.SetBool(_photoPosesParameterNameId, selector);
        }

        public void CallPhotoPose(int index)
        {
            if (index + 1 > _poseCount)
            {
                Debug.LogError("There is no pose with this index. Maximum 2");
                index = _poseCount - 1;
            }
            _animator.SetFloat(_poseSelectorParameterNameId, index);
            CallPhotoPose(true);
        }

        private void Start()
        {
            _photoPosesParameterNameId = Animator.StringToHash(_photoPosesParameterName);
            _poseSelectorParameterNameId = Animator.StringToHash(_poseSelectorParameterName);
        }

        private void Update()
        {
            DebugCheck();
        }

        private void DebugCheck()
        {
            if (!_debug)
                return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CallPhotoPose(0);
                Debug.Log("Женщина приняла позу 1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CallPhotoPose(1);
                Debug.Log("Женщина приняла позу 2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CallPhotoPose(2);
                Debug.Log("Женщина приняла позу 3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CallPhotoPose(false);
                Debug.Log("Женщина прекратила принимать позы");
            }
        }
    }
}