using UnityEngine;

namespace InteractionSystem
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float rayDistance = 2.5f;

        private KeyCode interactionKey => Main.MainManagers.settingsManager.InputConfig().Interaction_Key;

        private Camera _camera;
        private InteractableObject _lastPointed;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            ShootRaycast();
        }

        private void ShootRaycast()
        {
            if (_camera == null) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            InteractableObject interactableObject = null;
            if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer) && hit.transform.gameObject.TryGetComponent(out interactableObject))
            {
                if (_lastPointed != interactableObject)
                {
                    if (_lastPointed != null)
                    {
                        Main.MainManagers.interactionManager.OnPointerExit(_lastPointed);
                        _lastPointed = null;
                    }
                    Main.MainManagers.interactionManager.OnPointerEnter(interactableObject);
                }
                _lastPointed = interactableObject;
                if (!Input.GetKeyDown(interactionKey) && interactableObject.ForcedCall == false)
                    return;
                Main.MainManagers.interactionManager.ProcessInteraction(interactableObject);
            }
            else if (_lastPointed != null)
            {
                Main.MainManagers.interactionManager.OnPointerExit(_lastPointed);
                _lastPointed = null;
            }
            
        }
    }
}