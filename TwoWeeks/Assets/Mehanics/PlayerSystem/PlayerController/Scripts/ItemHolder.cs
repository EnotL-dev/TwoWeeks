using InteractionSystem;
using UnityEngine;

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
            }
        }
        private ItemObject _item;
        public Transform Target => _target;
        [SerializeField] private Transform _target;
        [SerializeField] private Camera _camera;

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