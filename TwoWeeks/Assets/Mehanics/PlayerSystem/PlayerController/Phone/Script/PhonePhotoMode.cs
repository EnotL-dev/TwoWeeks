using Environment;
using System;
using System.Collections;
using Tyan;
using UnityEngine;

namespace PlayerSystem
{
    public class PhonePhotoMode : MonoBehaviour
    {
        public Action OnStart;
        public Action OnStop;

        [SerializeField] private GameObject _phoneView;
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _debug = false;
        [SerializeField] private int _countToEnd = 3;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _flashDuration = 0.5f;
        [SerializeField] private float _photoShowingDuration = 0.5f;
        [SerializeField] private float _waitBeforeShowingDuration = 0.5f;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ShaderChanger _shaderFlash;
        [SerializeField] private ShaderChanger _shaderPhoto1;
        [SerializeField] private ShaderChanger _shaderPhoto2;
        [SerializeField] private ShaderChanger _shaderPhoto3;
        private int _poseIndex = 0;
        private bool _photing = false;
        private bool _flash = false;
        private Coroutine _activAction;

        private void Update()
        {
            DebugCheck();
            CheckPhoto();
        }

        private void Start()
        {
            _shaderFlash = new ShaderChanger("_Flash", _renderer);
            _shaderFlash.SetShaderParameter(true);

            _shaderPhoto1 = new ShaderChanger("_PhotoDemo1", _renderer);
            _shaderPhoto1.SetShaderParameter(true);

            _shaderPhoto2 = new ShaderChanger("_PhotoDemo2", _renderer);
            _shaderPhoto2.SetShaderParameter(true);

            _shaderPhoto3 = new ShaderChanger("_PhotoDemo3", _renderer);
            _shaderPhoto3.SetShaderParameter(true);
        }

        private void DebugCheck()
        {
            if (!_debug)
                return;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartPhotos();
            }
        }

        public void ShowPhone()
        {
            _phoneView.SetActive(true);
            _photing = true;
        }

        public void HidePhone()
        {
            _phoneView.SetActive(false);
            _photing = false;
        }

        public void StartPhotos()
        {
            TyanRefs.Instance.Animation.CallPhotoPose(true);
            TyanRefs.Instance.Animation.CallPhotoPose(0);
            TyanRefs.Instance.Emotion.SelectCatFace();
            Debug.Log("Start photing");
            ShowPhone();
            OnStart?.Invoke();
        }

        public void EndPhotos()
        {
            _photing = false;
            StartCoroutine(ShowPhotos());
        }

        private void CheckPhoto()
        {
            if (_photing && !_flash)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = new Ray(_phoneView.transform.position, -_phoneView.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Debug.Log(hit.transform.root.gameObject);
                        if (hit.transform.root.gameObject.TryGetComponent(out TyanRefs tyan))
                        {
                            AddPoint();
                            if (_activAction != null)
                                StopCoroutine(_activAction);
                            _activAction = StartCoroutine(DoFlash());
                        }
                    }
                }
            }
        }

        private void AddPoint()
        {
            Debug.Log("Next");
            _countToEnd--;
            TyanRefs.Instance.Emotion.SelectNormalFace();
            _poseIndex++;
            if (_countToEnd <= 0)
            {
                EndPhotos();
                Debug.Log("Complete photing");
                return;
            }
            TyanRefs.Instance.Animation.CallPhotoPose(_poseIndex);
        }

        private IEnumerator DoFlash()
        {
            _flash = true;
            _shaderFlash.SetShaderParameter(false);
            yield return new WaitForSeconds(_flashDuration);
            _shaderFlash.SetShaderParameter(true);
            _flash = false;
        }

        private IEnumerator ShowPhotos()
        {
            yield return new WaitForSeconds(_waitBeforeShowingDuration);
            Debug.Log("Start photo showing");
            _shaderPhoto1.SetShaderParameter(false);
            yield return new WaitForSeconds(_photoShowingDuration);
            yield return new WaitUntil(()=> Input.GetMouseButtonDown(0));
            _shaderPhoto1.SetShaderParameter(true);
            _shaderPhoto2.SetShaderParameter(false);
            yield return new WaitForSeconds(_photoShowingDuration);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            _shaderPhoto2.SetShaderParameter(true);
            _shaderPhoto3.SetShaderParameter(false);
            yield return new WaitForSeconds(_photoShowingDuration);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            OnStop?.Invoke();
            HidePhone();
            Debug.Log("End photo showing");
        }
    }
}