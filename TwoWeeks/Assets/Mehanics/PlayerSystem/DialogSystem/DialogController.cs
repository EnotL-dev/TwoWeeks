using InteractionSystem;
using TMPro;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    public class DialogController : MonoBehaviour
    {
        [Header("Check for move UI in dialogs")]
        [SerializeField] private LayerMask interactableLayer; //сюда ссылается dialogUI 
        [SerializeField] private float rayDistance = 5f; //сюда ссылается dialogUI 
        [Header("Main UI")]
        public CanvasGroup canvasGroup_MainUI;
        public TextMeshProUGUI textStart_MainUI;
        public TextMeshProUGUI textEnd_MainUI;
        [Header("Head UI")]
        public CanvasGroup prefabHeadUI;
        [HideInInspector] public TextMeshProUGUI textStart_HeadUI;
        [HideInInspector] public TextMeshProUGUI textEnd_HeadUI;

        private Camera _camera;

        public Camera GetCamera()
        {
            return _camera;
        }

        public LayerMask GetInteractableLayer()
        {
            return interactableLayer;
        }

        public float GetRayDistance()
        {
            return rayDistance;
        }

        private void Start()
        {
            _camera = GetComponent<Camera>();
            textStart_HeadUI = Instantiate(prefabHeadUI).GetComponentInChildren<TextMeshProUGUI>();
            textEnd_HeadUI = Instantiate(prefabHeadUI).GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
