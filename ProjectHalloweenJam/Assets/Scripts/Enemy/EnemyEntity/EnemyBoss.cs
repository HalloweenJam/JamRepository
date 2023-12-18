using Enemy.EnemyEntity;
using System;
using UnityEngine;

public class EnemyBoss : EnemyStats
{
    [SerializeField] private Transform _playerTransform;

    public static Action SecondPhase;
    public Action<float> OnTakeDamage;

    private bool _secondPhaseActive = false;

    public void Start() => base.Initialize(_playerTransform);

    public override bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
    {
        OnTakeDamage?.Invoke((float)CurrentHealth / MaxHealth);

        if ((((float)CurrentHealth / MaxHealth) * 100) <= 50f && !_secondPhaseActive)
        {
            _secondPhaseActive = true; 
            SecondPhase?.Invoke();
        }
        

        return base.TryTakeDamage(damage, instantKill, ignoreInvisibility);
    }
}
