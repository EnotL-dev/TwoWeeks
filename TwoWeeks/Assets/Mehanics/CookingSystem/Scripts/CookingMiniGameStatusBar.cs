using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGames
{
    public class CookingMiniGameStatusBar : MonoBehaviour
    {
        [SerializeField] private float _sliderBoarders;
        [SerializeField] private Image _fillingImage;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private CookingMinigame _minigame;
        private int _maxPoints = 1;
        private int _maxWeight = 1;

        private void UpdateSlider(int oldPoints, int currentPoints)
        {
            _arrow.anchoredPosition = new Vector3((((float)currentPoints + (float)_maxPoints) / ((float)_maxPoints * 2)) * _sliderBoarders * 2 - _sliderBoarders, _arrow.anchoredPosition.y);
        }

        private void UpdateFilling(int oldWeight, int currentWeight)
        {
            _fillingImage.fillAmount = (float)currentWeight / (float)_maxWeight;
        }

        private void OnEnable()
        {
            _maxPoints = _minigame.MaxPoints;
            _maxWeight = _minigame.MaxWeight;
            _minigame.Points.Changed += UpdateSlider;
            _minigame.Weight.Changed += UpdateFilling;
        }

        private void OnDisable()
        {
            _minigame.Points.Changed -= UpdateSlider;
            _minigame.Weight.Changed -= UpdateFilling;
        }
    }
}