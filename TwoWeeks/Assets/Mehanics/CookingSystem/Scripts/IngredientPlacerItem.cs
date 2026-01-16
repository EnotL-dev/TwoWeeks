using Environment;
using InteractionSystem;
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
        [SerializeField] private CanInteractEffect _interactEffect;
        [SerializeField] private ItemObject _itemObject;
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
                _last = item as CookingIngredient;
                CookingMinigame.Instance.AddIngredient(_last);
                _interactEffect.Deactivate();
                _itemObject.enabled = false;
                OnPlaceIngredient?.Invoke(_last);
            }
            base.Place();
        }
    }
}