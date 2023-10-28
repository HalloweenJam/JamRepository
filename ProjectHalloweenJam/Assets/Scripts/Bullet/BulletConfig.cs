using System;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class BulletConfig
    {
        public enum BulletType
        {
            line, sin, circle, fraction, firework,
        }
        public BulletType Type;
        public Sprite Sprite;
        public int Damage;
        public float Speed;
        public float Size;
        public float LifeTime;
        public int Coint;
        public int Radius;

    }
}
