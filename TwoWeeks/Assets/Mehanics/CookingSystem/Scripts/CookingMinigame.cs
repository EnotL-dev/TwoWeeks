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
        [SerializeField] private List<ItemObject> _interactionObjects = new();

        public void StartMiniGame()
        {
            foreach (var item in _interactionObjects) 
            {
                item.enabled = true;
            }
            _questController.AddQuest(_questPan);
        }

        public void NextQuest()
        {
            _questController.CompleteQuest(_questPan);
            _questController.AddQuest(_questIngridients);
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
            foreach (var item in _interactionObjects)
            {
                item.enabled = false;
            }
        }
    }
}