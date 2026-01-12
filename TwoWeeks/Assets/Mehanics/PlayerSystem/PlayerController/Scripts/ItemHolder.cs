using InteractionSystem;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
    public class ItemHolder : MonoBehaviour
    {
        public ItemObject Item { get => _item;
            set 
            {
                if (value != null && value.TryGetComponent(out Collider collider))
                {
                    collider.enabled = false;
                }
                _item = value;
                if (value!= null)
                {
                    OnGetInHands?.Invoke();
                }
                else
                {
                    OnGetOutHands?.Invoke();
                }
            }
        }
        private ItemObject _item;
        public Transform Target => _target;
        [SerializeField] private Transform _target;
        [SerializeField] private Camera _camera;
        [SerializeField] private UnityEvent OnGetInHands;
        [SerializeField] private UnityEvent OnGetOutHands;

        public ItemObject Extract()
        {
            if (Item == null)
            {
                Debug.LogError("No Item in hand");
                return null;
            }
            if (Item.TryGetComponent(out Collider collider))
            {
                collider.enabled = true;
            }
            var temp = Item;
            Item = null;
            return temp;
        }

        private void Update()
        {
            Vector3 rotation = _camera.transform.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}