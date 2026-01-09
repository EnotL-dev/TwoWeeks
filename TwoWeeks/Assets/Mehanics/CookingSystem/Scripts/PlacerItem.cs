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
            item.gameObject.SetActive(false);
            OnPlace?.Invoke();
            gameObject.SetActive(false);
        }
    }
}