using UnityEngine;

namespace Tyan
{
    public class TailGravityEffect : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction = Vector3.zero;
        
        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
}