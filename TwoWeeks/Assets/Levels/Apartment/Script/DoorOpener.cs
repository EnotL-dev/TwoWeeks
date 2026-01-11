using System.Collections;
using UnityEngine;

namespace Environment
{
    public class DoorOpener : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Vector3 _openRotationIn = new Vector3(0, 90, 0);
        [SerializeField] private Vector3 _openRotationOut = new Vector3(0, -90, 0);
        [SerializeField] private Transform _door;
        private bool _isOpen = false;
        private Quaternion _closedRotation;
        private Coroutine _coroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterController character))
            {
                if (transform.InverseTransformPoint(character.transform.position).x > 0)
                {
                    Debug.Log($"In {transform.InverseTransformPoint(character.transform.position)}");
                    Toggle(_openRotationIn, false);
                }
                else if (transform.InverseTransformPoint(character.transform.position).x < 0)
                {
                    Debug.Log($"Out {transform.InverseTransformPoint(character.transform.position)}");
                    Toggle(_openRotationOut, false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out CharacterController character))
            {
                Toggle(_openRotationIn, true);
            }
        }

        public void Toggle(Vector3 target, bool open)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (open)
                _coroutine = StartCoroutine(OpenClose(_closedRotation));
            else
                _coroutine = StartCoroutine(OpenClose(Quaternion.Euler(target) * _closedRotation));
        }

        private void Start()
        {
            _closedRotation = _door.transform.rotation;
        }

        private IEnumerator OpenClose(Quaternion targetRotation)
        {
            float _animationTimer = 0;
            float easedProgress = 0;
            while (_animationTimer < _animationDuration)
            {
                _animationTimer += Time.deltaTime;
                easedProgress = _curve.Evaluate(Mathf.Clamp01(_animationTimer / _animationDuration));
                _door.transform.rotation = Quaternion.Slerp(
                    _door.transform.rotation,
                    targetRotation,
                    easedProgress
                );
                yield return new WaitForEndOfFrame();
            }
        }
    }
}