using System;
using UnityEngine;
using System.Collections;

namespace Tyan
{
    [Serializable]
    public class EyesStates
    {
        public GameObject Opened => _opened;
        public GameObject Closed => _closed;    
        [SerializeField] private GameObject _opened;
        [SerializeField] private GameObject _closed;
    }

    [Serializable]
    public class MouthStates
    {
        public GameObject MouthNormalOpened => _mouthNormalOpened;
        public GameObject MouthNormalClosed => _mouthNormalClosed;
        public GameObject MouthCatOpened => _mouthCatOpened;
        public GameObject MouthCatClosed => _mouthCatClosed;
        [SerializeField] private GameObject _mouthNormalOpened;
        [SerializeField] private GameObject _mouthNormalClosed;
        [SerializeField] private GameObject _mouthCatOpened;
        [SerializeField] private GameObject _mouthCatClosed;
    }

    public class EmotionSwitcher : MonoBehaviour
    {
        [SerializeField] private EyesStates _eyesStates;
        [SerializeField] private float _eyesOpenTime;
        [SerializeField] private float _eyesCloseTime;
        [SerializeField] private float _mouthSpeakFrequency;
        [SerializeField] private MouthStates _mouthStates;
        [Space]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _browsParameterName;
        [SerializeField] private bool _debug = false;
        private int _browsParameterId;
        private Coroutine _blinkingCoroutine;
        private Coroutine _speakingCoroutine;
        private GameObject _mouthOpened;
        private GameObject _mouthClosed;

        private void Update()
        {
            DebugCheck();
        }

        private void DebugCheck()
        {
            if (!_debug)
                return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartBlinking();
                Debug.Log("Женщина начала моргать");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StopBlinking();
                Debug.Log("Женщина прекратила моргать");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartSpeaking();
                Debug.Log("Женщина начала говорить");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StopSpeaking();
                Debug.Log("Женщина прекратила говорить");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectCatFace();
                Debug.Log("Режим кошкожены активирован");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SelectNormalFace();
                Debug.Log("Режим кошкожены отключён");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                BrowsSurprise();
                Debug.Log("Женщина удивлена");
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                BrowsNormal();
                Debug.Log("Женщине норм");
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                BrowsAngry();
                Debug.Log("Женщина злая");
            }
        }

        public void Start()
        {
            _browsParameterId = Animator.StringToHash(_browsParameterName);
            SelectNormalFace();
        }

        public void StartBlinking()
        {
            if (_blinkingCoroutine != null)
                StopCoroutine(_blinkingCoroutine);
            _blinkingCoroutine = StartCoroutine(DoBlink());
        }

        public void StopBlinking()
        {
            if (_blinkingCoroutine != null)
                StopCoroutine(_blinkingCoroutine);
            OpenEyes();
        }

        public void OpenEyes()
        {
            _eyesStates.Opened.SetActive(true);
            _eyesStates.Closed.SetActive(false);
        }

        public void CloseEyes()
        {
            _eyesStates.Opened.SetActive(false);
            _eyesStates.Closed.SetActive(true);
        }

        private IEnumerator DoBlink()
        {
            while (true) 
            {
                OpenEyes();
                yield return new WaitForSeconds(_eyesOpenTime);
                CloseEyes();
                yield return new WaitForSeconds(_eyesCloseTime);
            }
        }

        public void SelectNormalFace()
        {
            _mouthStates.MouthNormalClosed.SetActive(true);
            _mouthStates.MouthNormalOpened.SetActive(false);
            _mouthStates.MouthCatClosed.SetActive(false);
            _mouthStates.MouthCatOpened.SetActive(false);
            _mouthOpened = _mouthStates.MouthNormalOpened;
            _mouthClosed = _mouthStates.MouthNormalClosed;
        }

        public void SelectCatFace()
        {
            _mouthStates.MouthNormalClosed.SetActive(false);
            _mouthStates.MouthNormalOpened.SetActive(false);
            _mouthStates.MouthCatClosed.SetActive(true);
            _mouthStates.MouthCatOpened.SetActive(false);
            _mouthOpened = _mouthStates.MouthCatOpened;
            _mouthClosed = _mouthStates.MouthCatClosed;
        }

        public void OpenMouth()
        {
            _mouthOpened.SetActive(true);
            _mouthClosed.SetActive(false);
        }

        public void CloseMouth()
        {
            _mouthOpened.SetActive(false);
            _mouthClosed.SetActive(true);
        }

        public void StartSpeaking()
        {
            if (_speakingCoroutine != null)
                StopCoroutine(_speakingCoroutine);
            _speakingCoroutine = StartCoroutine(DoSpeak());
        }

        public void StopSpeaking()
        {
            if (_speakingCoroutine != null)
                StopCoroutine(_speakingCoroutine);
            CloseMouth();
        }

        private IEnumerator DoSpeak()
        {
            while (true)
            {
                OpenMouth();
                yield return new WaitForSeconds(_mouthSpeakFrequency);
                CloseMouth();
                yield return new WaitForSeconds(_mouthSpeakFrequency);
            }
        }

        public void BrowsSurprise()
        {
            _animator.SetFloat(_browsParameterId, 0);
        }

        public void BrowsNormal()
        {
            _animator.SetFloat(_browsParameterId, 1);
        }

        public void BrowsAngry()
        {
            _animator.SetFloat(_browsParameterId, 2);
        }
    }
}