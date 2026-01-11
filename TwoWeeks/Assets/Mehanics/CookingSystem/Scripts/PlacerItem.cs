using UnityEngine;
using UnityEngine.Events;

namespace MiniGames
{
    public class PlacerItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPlace;

        public void Place()
        {
            var item = Main.MainControllers.playerController.itemHolder.Extract();
            if (item == null)
                return;
            Destroy(item.gameObject);
            OnPlace?.Invoke();
        }
    }
}