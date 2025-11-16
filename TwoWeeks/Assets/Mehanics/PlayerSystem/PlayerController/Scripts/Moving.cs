using ReactiveVariables;
using UnityEngine;

namespace PlayerSystem
{
    public class Moving : MonoBehaviour
    {
        public float Magnitude { get => _characterController.velocity.magnitude; }
        public ReactiveProperty<bool> IsSprint = new(true);
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private Camera _camera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Jumping _jumpingSystem;
        [SerializeField] private bool _debug = false;
        private Vector3 _moving;

        private void Update()
        {
            if (Time.timeScale < 1)
                return;

            Move();
        }

        private void Sprint()
        {
            IsSprint.Value = Input.GetKey(KeyCode.LeftShift);
        }

        private float GetSpeed()
        {
            return (IsSprint.Value ? _config.SprintSpeed : _config.WalkingSpeed);
        }

        private void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 cameraRight = _camera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 _inputMoving =
                cameraRight * x +
                cameraForward * z;
            _inputMoving = Vector3.ClampMagnitude(_inputMoving, 1);
            if (_jumpingSystem.IsGrounded.Value)
            {
                _moving = _inputMoving * GetSpeed();
                _characterController.Move(_moving * Time.deltaTime);
                Sprint();
            }
            else
            {
                _moving += _inputMoving * _config.MovingSpeedJump * Time.deltaTime;
                _moving = Vector3.ClampMagnitude(_moving, GetSpeed());
                _characterController.Move(_moving * Time.deltaTime);
            }
            if (_debug)
                Debug.Log($"player speed: " + _characterController.velocity.magnitude);
        }
    }
}