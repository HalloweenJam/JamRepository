using System;
using Core.Classes;
using Core.Enums;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class BulletConfig
    {
        [SerializeField] private BulletType _bulletType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private OptionalProperty<Material> _customMaterial;
        [SerializeField] private OptionalProperty<RuntimeAnimatorController> _animatorController;
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

        public Material DefaultMaterial => _defaultMaterial;
        public OptionalProperty<Material> CustomMaterial => _customMaterial;
        public OptionalProperty<RuntimeAnimatorController> AnimatorController => _animatorController;

        public float Scale => _scale;
        public bool IsEnemyBullet => _isEnemyBullet;
        public float Speed => _speed;
        public float Radius => _radius;
        
        public int Damage => _damage;
        public int Count => _count;

        public void SetSprite(Sprite sprite) => _sprite = sprite;
    }
}
