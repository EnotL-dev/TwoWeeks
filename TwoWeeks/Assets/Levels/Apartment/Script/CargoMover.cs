using System.Collections;
using UnityEngine;

namespace Environment
{
    public class CargoMover : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Vector3 _movePosition = new Vector3(0, 1, 0);
        private bool _isOpen = false;
        private Vector3 _closed;
        private Coroutine _coroutine;

        public void Toggle()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (_isOpen)
                _coroutine = StartCoroutine(OpenClose(_closed));
            else
                _coroutine = StartCoroutine(OpenClose(_movePosition + _closed));
            _isOpen = !_isOpen;
        }

        private void Start()
        {
            _closed = transform.position;
        }

        private IEnumerator OpenClose(Vector3 target)
        {
            float _animationTimer = 0;
            float easedProgress = 0;
            while (_animationTimer < _animationDuration)
            {
                _animationTimer += Time.deltaTime;
                easedProgress = _curve.Evaluate(Mathf.Clamp01(_animationTimer / _animationDuration));
                transform.position = Vector3.Slerp(
                    transform.position,
                    target,
                    easedProgress
                );
                yield return new WaitForEndOfFrame();
            }
        }
    }
}