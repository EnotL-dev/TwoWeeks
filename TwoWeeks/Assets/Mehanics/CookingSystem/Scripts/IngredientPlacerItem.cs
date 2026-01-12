using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGames
{
    [Serializable]
    public class IngredientEvent : UnityEvent<CookingIngredient> { }
    public class IngredientPlacerItem : PlacerItem
    {
        [SerializeField] private IngredientEvent OnPlaceIngredient;
        private CookingIngredient _last;

        public override void Place()
        {
            var item = Main.MainControllers.playerController.itemHolder.Item.item;
            if (item is CookingIngredient)
            {
                if (_last != null && _last == item)
                {
                    return;
                }
                OnPlaceIngredient?.Invoke(item as CookingIngredient);
                _last = item as CookingIngredient;
            }
            base.Place();
        }
    }
}