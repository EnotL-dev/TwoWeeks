using UnityEngine;
using UnityEngine.Events;

namespace MiniGames
{
    public class PlacerItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPlace;

        public void Place()
        {
            Destroy(Main.MainControllers.playerController.itemHolder.Extract().gameObject);
            OnPlace?.Invoke();
            gameObject.SetActive(false);
        }
    }
}