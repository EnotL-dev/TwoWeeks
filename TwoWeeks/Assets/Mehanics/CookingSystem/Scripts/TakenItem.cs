using InteractionSystem;
using UnityEngine;
namespace MiniGames
{
    [RequireComponent(typeof(ItemObject))]
    public class TakenItem : MonoBehaviour
    {
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _offsetRotation;
        [SerializeField] private bool _hideAfter = true;
        private ItemObject _itemObject;

        public void Take()
        {
            var instance = Instantiate(_itemObject.item.Prefab, transform.position, Quaternion.identity);
            Main.MainControllers.playerController.itemHolder.Item = instance;
            instance.transform.parent = Main.MainControllers.playerController.itemHolder.Target;
            instance.transform.localPosition = _offsetPosition;
            instance.transform.localRotation = Quaternion.Euler(_offsetRotation);
            if (_hideAfter)
            {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _itemObject = GetComponent<ItemObject>();
        }
    }
}