using InteractionSystem;
using PlayerSystem.QuestSystem;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGames
{
    public class CookingMinigame : MonoBehaviour
    {
        [SerializeField] private QuestController _questController;
        [SerializeField] private Quest _questPan;
        [SerializeField] private Quest _questIngridients;
        [SerializeField] private List<ItemObject> _cabinets = new();
        [SerializeField] private List<ItemObject> _cookware = new();
        [SerializeField] private List<ItemObject> _dishes = new();
        [SerializeField] private List<ItemObject> _ingredients = new();

        public void StartMiniGame()
        {
            UnblockCabinets();
            UnblockCookware();
            _questController.AddQuest(_questPan);
        }

        public void QuestStartCooking()
        {
            _questController.CompleteQuest(_questPan);
            _questController.AddQuest(_questIngridients);
            UnblockIngredients();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartMiniGame();
            }
        }

        private void Start()
        {
            foreach (var item in _cabinets)
            {
                item.enabled = false;
            }
        }

        private void UnblockCabinets()
        {
            foreach (var item in _cabinets)
            {
                item.enabled = true;
            }
        }

        private void UnblockCookware()
        {
            foreach (var item in _cookware)
            {
                item.enabled = true;
            }
        }

        private void UnblockIngredients()
        {
            foreach (var item in _ingredients)
            {
                item.enabled = true;
            }
        }
    }
}