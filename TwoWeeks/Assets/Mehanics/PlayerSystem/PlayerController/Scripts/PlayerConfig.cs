using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Create PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public float WalkingSpeed => _walkingSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float MovingSpeedJump => _movingSpeedJump;
        public float JumpHeight => _jumpHeight;
        public float JumpHeightSecond => _jumpHeightSecond;
        public float Gravity => _gravity;
        public float GravityOnGround => _gravityOnGround;
        public float GroundCheckDistance => _groundCheckDistance;
        public LayerMask GroundCheckMask => _groundCheckMask;
        public float TimeToSecondJump => _timeToSecondJump;
        public MinMaxValue StepSoundFrequency => _stepSoundFrequency;
        [SerializeField] private float _walkingSpeed = 10f;
        [SerializeField] private float _sprintSpeed = 15f;
        [SerializeField] private float _movingSpeedJump = 10f;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _jumpHeightSecond = 5f;
        [SerializeField] private float _gravity = 1f;
        [SerializeField] private float _gravityOnGround = -2f;
        [SerializeField] private float _groundCheckDistance = 1f;
        [SerializeField] private LayerMask _groundCheckMask;
        [SerializeField] private float _timeToSecondJump;
        [SerializeField] private MinMaxValue _stepSoundFrequency = new();
    }
}