using UnityEngine;

namespace Tyan
{
    public class TyanIkController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _bodyWeight = 0.3f;
        [SerializeField] private float _headWeight = 1.0f;
        [SerializeField] private float _eyesWeight = 0f;
        [SerializeField] private float _clampWeight = 0.5f;
        private float _totalWeight = 1f;
        private bool _body = false;
        private bool _head = true;

        public void ActivateIk(float weight, bool body, bool head)
        {
            _totalWeight = weight;
            _body = body;
            _head = head;
        }

        public void DeactivateIk()
        {
            _totalWeight = 0f;
            _body = false;
            _head = false;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (_target != null)
            {
                _animator.SetLookAtWeight(_totalWeight, _body ? _bodyWeight : 0f, _head ? _headWeight : 0f, _eyesWeight, _clampWeight);
                _animator.SetLookAtPosition(_target.position);
            }
            else
            {
                _animator.SetLookAtWeight(0f);
            }
        }
    }
}