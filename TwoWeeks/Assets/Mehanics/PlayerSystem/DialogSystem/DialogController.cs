using InteractionSystem;
using TMPro;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    public class DialogController : MonoBehaviour
    {
        [Header("Check for move UI in dialogs")]
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float rayDistance = 5f;
        private float moveDistance = 1.4f;
        private float minDistanceToWall = 0.5f;
        [Header("Main UI")]
        public CanvasGroup canvasGroup_MainUI;
        public TextMeshProUGUI textStart_MainUI;
        public TextMeshProUGUI textEnd_MainUI;
        [Header("Head UI")]
        public CanvasGroup prefabHeadUI;
        [HideInInspector] public TextMeshProUGUI textStart_HeadUI;
        [HideInInspector] public TextMeshProUGUI textEnd_HeadUI;

        private Camera _camera;
        private void Start()
        {
            _camera = GetComponent<Camera>();
            textStart_HeadUI = Instantiate(prefabHeadUI).GetComponentInChildren<TextMeshProUGUI>();
            textEnd_HeadUI = Instantiate(prefabHeadUI).GetComponentInChildren<TextMeshProUGUI>();
        }

        private bool CheckLogicForMoveCanvas()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
            {
                if (hit.transform.gameObject.GetComponent<InteractableObject>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryMoveHeadCanvasToCameraView()
        {
            if (CheckLogicForMoveCanvas())
            {
                MoveHeadCanvasToCameraView();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MoveHeadCanvasToCameraView()
        {
            Vector3 offset = new Vector3(0, -0.4f, 0);
            Vector3 cameraPos = transform.position + offset;
            Vector3 direction = transform.forward;

            if (Physics.Raycast(cameraPos, direction, out RaycastHit hit, moveDistance))
            {
                textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = hit.point - direction * minDistanceToWall;
                textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = hit.point - direction * minDistanceToWall;
            }
            else
            {
                textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = cameraPos + direction * moveDistance;
                textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = cameraPos + direction * moveDistance;
            }

            textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.LookAt(textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position + transform.rotation * Vector3.forward,
                                   transform.rotation * Vector3.up);
            textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.LookAt(textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position + transform.rotation * Vector3.forward,
                                   transform.rotation * Vector3.up);
        }
    }
}
