using UnityEngine;

public class TurnToFacePlayer : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private bool _yDirection = true;
    [SerializeField] private bool _speed = true;
    [SerializeField] private float _yOffset;
    private Transform playerTransform;
    private Vector3 _lookDirection;

    private void Start()
    {
        playerTransform = Main.MainControllers.playerController.moving.transform;
    }

    public void Update()
    {
        Vector3 playerPosition = playerTransform.transform.position;
        playerPosition.y += _yOffset;
        _lookDirection = playerPosition - transform.position;
        if (!_yDirection)
            _lookDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);
        if (_speed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(playerPosition);
        }
    }
}