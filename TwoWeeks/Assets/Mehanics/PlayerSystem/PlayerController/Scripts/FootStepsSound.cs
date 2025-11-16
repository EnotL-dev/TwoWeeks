using UnityEngine;

namespace PlayerSystem
{
    public class FootStepsSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _footsteps;
        [SerializeField] private Moving _moving;
        [SerializeField] private Jumping _jumping;
        [SerializeField] private PlayerConfig _config;
        private float _timeToStep;
        private float _currentTimeToStep = 0;
        private bool _left = true;

        private void OnEnable()
        {
            _moving.IsSprint.Changed += ChangeTimeToStep;
            ChangeTimeToStep(false, false);
        }

        private void OnDisable()
        {
            _moving.IsSprint.Changed -= ChangeTimeToStep;
        }

        private void Update()
        {
            if (_jumping.IsGrounded.Value && _moving.Magnitude > 0)
            {
                _currentTimeToStep += Time.deltaTime;
                if (_currentTimeToStep > _timeToStep)
                {
                    _currentTimeToStep = 0;
                    _footsteps.pitch = GetPitch();
                    _footsteps.Play();
                }
            }
            //else
            //{
            //    _currentTimeToStep = _timeToStep + 1;
            //}
        }

        private float GetPitch()
        {
            if (_left)
            {
                _left = false;
                return 0.9f;
            }
            else
            {
                _left = true;
                return 1f;
            }
            //return 1f;
        }

        private void ChangeTimeToStep(bool old, bool isRun)
        {
            _timeToStep = (isRun) ?
                _config.StepSoundFrequency.Min
                : _config.StepSoundFrequency.Max;
        }
    }
}