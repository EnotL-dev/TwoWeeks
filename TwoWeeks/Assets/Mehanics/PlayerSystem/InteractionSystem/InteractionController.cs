using UnityEngine;

namespace InteractionSystem
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float rayDistance = 2.5f;

        private KeyCode interactionKey => Main.MainManagers.settingsManager.InputConfig().Interaction_Key;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactionKey))
            {
                ShootRaycast();
            }
        }
        private void ShootRaycast()
        {
            if (_camera == null) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
            {
                if(hit.transform.gameObject.GetComponent<InteractableObject>() != null)
                    Main.MainManagers.interactionManager.ProcessInteraction(hit.transform.gameObject.GetComponent<InteractableObject>());
            }
        }
    }
}