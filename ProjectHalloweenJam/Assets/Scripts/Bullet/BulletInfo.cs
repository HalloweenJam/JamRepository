using System;
using UnityEngine;

namespace Core.Classes
{
    [Serializable]
    public class BulletInfo
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
