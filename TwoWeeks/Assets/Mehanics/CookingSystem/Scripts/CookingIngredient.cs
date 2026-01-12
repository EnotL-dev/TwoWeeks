using InteractionSystem;
using UnityEngine;

namespace MiniGames
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "Items/Ingredient")]
    public class CookingIngredient : Item
    {
        public int Weight => _weight;
        public int Points => _points;

        [SerializeField] private int _weight = 1;
        [SerializeField] private int _points = 1;
    }
}