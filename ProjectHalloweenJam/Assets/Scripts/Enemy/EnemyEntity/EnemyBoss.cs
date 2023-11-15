using Enemy.EnemyEntity;
using System;
using UnityEngine;

public class EnemyBoss : EnemyStats
{
    [SerializeField] private Transform _playerTransform;
    public Action<float> OnTakeDamage;

    public void Start() => base.Initialize(_playerTransform);

    public override bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
    {
        OnTakeDamage?.Invoke((float)CurrentHealth / MaxHealth);
        return base.TryTakeDamage(damage, instantKill, ignoreInvisibility);
    }
}
