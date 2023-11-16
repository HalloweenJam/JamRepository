using System;
using Core.Classes;
using Core.Enums;
using UnityEditor.Animations;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class BulletConfig
    {
        [SerializeField] private BulletType _bulletType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private OptionalProperty<AnimatorController> _animatorController;
        [SerializeField] private bool _isEnemyBullet = false;
        [SerializeField] private float _scale = 1f;
        [Space]
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [Space]
        [SerializeField] private int _count;
        [SerializeField] private float _radius;
        
        public BulletType Type => _bulletType;
        public Sprite Sprite => _sprite;
        public OptionalProperty<AnimatorController> AnimatorController => _animatorController;

        public float Scale => _scale;
        public bool IsEnemyBullet => _isEnemyBullet;

        public float Speed => _speed;
        public float Radius => _radius;
        
        public int Damage => _damage;
        public int Count => _count;
    }
}
