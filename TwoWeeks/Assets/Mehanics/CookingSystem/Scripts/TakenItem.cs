using InteractionSystem;
using UnityEngine;
namespace MiniGames
{
    public class TakenItem : MonoBehaviour
    {
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _offsetRotation;
        [SerializeField] private ItemObject _itemObject;

        public void Take()
        {
            Main.MainControllers.playerController.itemHolder.Item = _itemObject;
            transform.parent = Main.MainControllers.playerController.itemHolder.Target;
            transform.localPosition = _offsetPosition;
            transform.localRotation = Quaternion.Euler(_offsetRotation);
            _itemObject.OnPointerExit?.Invoke();
            _itemObject.enabled = false;
        }
    }
}