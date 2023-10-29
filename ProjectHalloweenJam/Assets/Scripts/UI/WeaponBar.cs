using Core.Classes;
using DG.Tweening;
using Entities;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponBar : MonoBehaviour
    {
        [Space(12)] 
        [SerializeField] private float _batchDuration = .05f;
        [SerializeField] private float _totalDuration = .1f;
        
        [SerializeField] private Image _batchFill;
        [SerializeField] private Image _totalFill;

        [SerializeField] private TextMeshProUGUI _bulletsInBatch;
        [SerializeField] private TextMeshProUGUI _totalBullets;

        [SerializeField] private Image _weaponImage;

        private WeaponSelector _player;
        
        private void Start()
        {
            _player = FindFirstObjectByType<PlayerStats>().GetComponent<WeaponSelector>();
            _player.OnWeaponUsed += ChangeBar;
        }

        private void OnDisable()
        {
            _player.OnWeaponUsed -= ChangeBar;
        }

        private void ChangeBar(WeaponData data, bool isChanged)
        {
            var inBatchRatio = (float) data.LeftBulletsInBatch / data.TotalBulletsInBatch;
            var totalRatio = (float) data.LeftBullets / data.TotalBullets;

            AnimateFillAmount(_batchFill, inBatchRatio, _batchDuration);

            if (data.TotalBullets > 0)
                AnimateFillAmount(_totalFill, totalRatio, _totalDuration);
            else
                _totalFill.fillAmount = 1;

            _bulletsInBatch.text = $"{data.LeftBulletsInBatch}/{data.TotalBulletsInBatch}";
            _totalBullets.text = data.TotalBullets < 0 ? "\u221e" : $"{data.LeftBullets}/{data.TotalBullets}";
       
            if (isChanged)
                _weaponImage.sprite = data.Icon;
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