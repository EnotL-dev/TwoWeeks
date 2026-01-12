using System.Collections;
using UnityEngine;

namespace Environment
{
    public class Opener : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Vector3 _openRotation = new Vector3(0, 90, 0);
        private bool _isOpen = false;
        private Quaternion _closedRotation;
        private Coroutine _coroutine;

        public void Toggle()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (_isOpen)
                _coroutine = StartCoroutine(OpenClose(_closedRotation));
            else
                _coroutine = StartCoroutine(OpenClose(Quaternion.Euler(_openRotation) * _closedRotation));
            _isOpen = !_isOpen;
        }

        public void Close()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (_isOpen)
                _coroutine = StartCoroutine(OpenClose(_closedRotation));
        }

        private void Start()
        {
            _closedRotation = transform.rotation;
        }

        private IEnumerator OpenClose(Quaternion targetRotation)
        {
            float _animationTimer = 0;
            float easedProgress = 0;
            while (_animationTimer < _animationDuration)
            {
                _animationTimer += Time.deltaTime;
                easedProgress = _curve.Evaluate(Mathf.Clamp01(_animationTimer / _animationDuration));
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    easedProgress
                );
                yield return new WaitForEndOfFrame();
            }
        }
    }
}