using UnityEngine;

namespace Tyan
{
    public class TyanIkController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Animator _animator;

        private void OnAnimatorIK(int layerIndex)
        {
            if (_target != null)
            {
                _animator.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
                _animator.SetLookAtPosition(_target.position);
            }
            else
            {
                _animator.SetLookAtWeight(0f);
            }
        }
    }
}