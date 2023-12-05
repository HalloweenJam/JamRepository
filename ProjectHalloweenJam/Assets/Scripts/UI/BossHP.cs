using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Core;

public class BossHP : MonoBehaviour
{
    [SerializeField] private float _stepSpeed = .25f;
    [SerializeField] private Image _hpFill;
    [SerializeField] private TextMeshProUGUI _nameBoss;
    [SerializeField] private EnemyBoss _bossStats;

    private void Start() => _bossStats.OnTakeDamage += (float ratio) => AnimateFillAmount(_hpFill, ratio, _stepSpeed);

    private static void AnimateFillAmount(Image fillImage, float targetFillAmount, float duration)
    {
        var currentFillAmount = fillImage.fillAmount;
        DOTween.To(() => currentFillAmount, x => currentFillAmount = x, targetFillAmount, duration)
            .SetEase(Ease.InOutQuad)
            .OnUpdate(() => fillImage.fillAmount = currentFillAmount);
    }

    public void ShowBossBaR()
    {
        _hpFill.Activate();
        _nameBoss.Activate();
    }

    private void OnDisable() => _bossStats.OnTakeDamage -= (float ratio) => AnimateFillAmount(_hpFill, ratio, _stepSpeed);
}
