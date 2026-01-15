using UnityEngine;

namespace PlayerSystem
{
    public class PhonePhotoMode : MonoBehaviour
    {
        [SerializeField] private GameObject _phoneView;
        [SerializeField] private bool _debug = false;

        private void Update()
        {
            DebugCheck();
        }

        private void DebugCheck()
        {
            if (!_debug)
                return;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ShowHidePhone();
            }
        }

        public void ShowHidePhone()
        {
            _phoneView.SetActive(!_phoneView.activeSelf);
        }

        public void ShowPhone()
        {
            _phoneView.SetActive(true);
        }

        public void HidePhone()
        {
            _phoneView.SetActive(false);
        }
    }
}