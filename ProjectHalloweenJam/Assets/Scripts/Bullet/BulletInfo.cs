using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System;

[Serializable]
public struct BulletInfo
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
    public float TimeHeart;
    public int Coint;
    public int Radius;

}
