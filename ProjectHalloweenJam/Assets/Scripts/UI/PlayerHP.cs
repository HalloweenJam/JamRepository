using System;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerHP : MonoBehaviour
    {
        [SerializeField] private float _stepSpeed = .25f;
        [SerializeField] private Image _hpFill;
        
        private PlayerStats _playerStats;
        
        private void Awake()
        {
            _playerStats = FindFirstObjectByType<PlayerStats>();
        }

        private void Start()
        {
            _playerStats.OnPlayerTakeDamage += OnPlayerTakeDamage;
            print(_playerStats.name);
        }

        private void OnPlayerTakeDamage(float ratio)
        {
            AnimateFillAmount(_hpFill, ratio, _stepSpeed);
        }
        
        private static void AnimateFillAmount(Image fillImage, float targetFillAmount, float duration)
        {
            var currentFillAmount = fillImage.fillAmount;
            DOTween.To(() => currentFillAmount, x => currentFillAmount = x, targetFillAmount, duration)
                .SetEase(Ease.InOutQuad)
                .OnUpdate(() => fillImage.fillAmount = currentFillAmount);
        }
    }
}