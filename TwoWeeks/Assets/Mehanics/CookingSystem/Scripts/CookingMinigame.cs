using Environment;
using InteractionSystem;
using PlayerSystem.QuestSystem;
using ReactiveVariables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGames
{
    public class CookingMinigame : MonoBehaviour
    {
        public static CookingMinigame Instance { get; private set; }
        public int MaxPoints => _maxPoints;
        public int MaxWeight => _maxWeight;
        public ReactiveProperty<int> Points = new();
        public ReactiveProperty<int> Weight = new();
        public ItemObject IngredientsCollector => _ingredientsCollector;
        [SerializeField] private int _startPoints;
        [SerializeField] private int _startWeight;
        [SerializeField] private int _maxPoints;
        [SerializeField] private int _maxWeight;
        [Space]
        [SerializeField] private UnityEvent winEvents;
        [Space]
        [SerializeField] private QuestController _questController;
        [SerializeField] private Quest _questPan;
        [SerializeField] private Quest _questIngridients;
        [SerializeField] private List<CargoMover> _cargo = new();
        [SerializeField] private List<Opener> _cabinets = new();
        [SerializeField] private List<ItemObject> _cookware = new();
        [SerializeField] private List<ItemObject> _ingredients = new();
        [SerializeField] private ItemObject _ingredientsCollector;

        public void AddIngredient(CookingIngredient ingredient)
        {
            Points.Value = Mathf.Clamp(Points.Value + ingredient.Points, -_maxPoints, _maxPoints);
            Weight.Value = Mathf.Clamp(Weight.Value + ingredient.Weight, -_maxWeight, _maxWeight);
            CheckWin();
        }

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
        
        private void CheckWin()
        {
            if (Points.Value == 0 && Weight.Value == _maxWeight)
            {
                Debug.Log("End cooking");
                _questController.CompleteQuest(_questIngridients);
                BlockCabinets();
                BlockIngredients();

                winEvents?.Invoke();
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Points.Value = _startPoints;
            Weight.Value = _startWeight;
            BlockCabinets();
        }

        private void UnblockCabinets()
        {
            foreach (var item in _cabinets)
            {
                item.GetComponent<ItemObject>().enabled = true;
            }
            foreach (var item in _cargo)
            {
                item.GetComponent<ItemObject>().enabled = true;
            }
        }

        private void BlockCabinets()
        {
            foreach (var item in _cabinets)
            {
                item.Close();
                item.GetComponent<ItemObject>().enabled = false;
            }
            foreach (var item in _cargo)
            {
                item.Close();
                item.GetComponent<ItemObject>().enabled = false;
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

        private void BlockIngredients()
        {
            foreach (var item in _ingredients)
            {
                item.enabled = false;
            }
        }
    }
}