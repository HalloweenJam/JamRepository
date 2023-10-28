using System;
using Core.Enums;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class BulletConfig
    {
        [SerializeField] private BulletType _bulletType;
        [SerializeField] private Sprite _sprite;
        [Space]
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [Space]
        [SerializeField] private int _count;
        [SerializeField] private float _radius;
        
        public BulletType Type => _bulletType;
        public Sprite Sprite => _sprite;

        public float Speed => _speed;
        public float Radius => _radius;
        
        public int Damage => _damage;
        public int Count => _count;
    }
}
