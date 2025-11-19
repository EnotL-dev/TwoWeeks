using InteractionSystem;
using TMPro;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    public class DialogController : MonoBehaviour
    {
        public CanvasGroup canvasGroup_MainUI;
        public TextMeshProUGUI textMesh_MainUI;
        [Space(10)]
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float rayDistance = 2.5f;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        public bool RaycastHitTarget()
        {
            if (_camera == null) return false;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
            {
                return hit.transform.gameObject.GetComponent<DialogObject>() != null;
            }
            else return false;
        }
    }
}
