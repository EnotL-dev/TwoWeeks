using ReactiveVariables;
using System;
using System.Collections;
using UnityEngine;

namespace PlayerSystem
{
    public class Jumping : MonoBehaviour
    {
        public event Action GroundedFromHeight;
        public ReactiveProperty<bool> IsGrounded = new(true);
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _groundCheckerPosition;
        [SerializeField] private AudioSource _audioStartJump;
        [SerializeField] private AudioSource _audioSecondJump;
        [SerializeField] private AudioSource _audioLanding;
        private Vector3 _velocity;
        private bool _secondJump = false;
        private bool _canSecondJump = false;
        private Coroutine _secondJumpTimer;
        private float _startJumpHight;

        private void Update()
        {
            CheckGround();
            ApplyGravitation();
            MoveVertical();
            StartJump();
        }

        private void OnEnable()
        {
            IsGrounded.Changed += Landing;
        }

        private void OnDisable()
        {
            IsGrounded.Changed -= Landing;
        }

        private void StartJump()
        {
            if (Time.timeScale < 1)
                return;

            if ((IsGrounded.Value || _canSecondJump) && CanJump() && Input.GetKeyDown(KeyCode.Space) && !_secondJump)
            {
                if (_canSecondJump)
                {
                    _secondJump = true;
                    _audioSecondJump.Play();
                }
                _velocity.y = Mathf.Sqrt((_secondJump ? _playerConfig.JumpHeightSecond : _playerConfig.JumpHeight) * -2f * _playerConfig.Gravity);
                if (!_secondJump)
                {
                    _secondJumpTimer = StartCoroutine(LaunchTimerToSecondJump());
                    _audioStartJump.Play();
                }
            }
        }

        private IEnumerator LaunchTimerToSecondJump()
        {
            yield return new WaitForSeconds(_playerConfig.TimeToSecondJump);
            _canSecondJump = true;
        }

        private void CheckGround()
        {
            IsGrounded.Value = Physics.CheckSphere(
                _groundCheckerPosition.position,
                _playerConfig.GroundCheckDistance,
                _playerConfig.GroundCheckMask);
            if (IsGrounded.Value && _velocity.y < 0)
                _velocity.y = _playerConfig.GravityOnGround;
        }

        private void ApplyGravitation()
        {
            _velocity.y += _playerConfig.Gravity * Time.deltaTime;
        }

        private void MoveVertical()
        {
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private bool CanJump()
        {
            return !CheckCapsule(
                _characterController.transform.position + Vector3.up * _playerConfig.JumpHeight,
                _characterController.height,
                _characterController.radius,
                _playerConfig.GroundCheckMask
                );
        }

        private bool CheckCapsule(Vector3 bottomPoint, float height, float radius, LayerMask layerMask)
        {
            Vector3 startPoint = bottomPoint;
            startPoint.y += radius;
            Vector3 endPosition = bottomPoint;
            endPosition.y += height - radius;
            return Physics.CheckCapsule(startPoint, endPosition, radius, layerMask);
        }

        private void Landing(bool oldValue, bool newValue)
        {
            if (newValue == false)
            {
                _startJumpHight = _characterController.transform.position.y;
            }
            if (newValue == true)
            {
                _secondJump = false;
                _canSecondJump = false;
                if (_secondJumpTimer != null)
                    StopCoroutine(_secondJumpTimer);

                if (_startJumpHight - _characterController.transform.position.y > 1f)
                {
                    GroundedFromHeight?.Invoke();
                    _audioLanding.Play();
                }
            }
            Debug.Log($"IsGrounded = {IsGrounded.Value}");
        }
    }
}