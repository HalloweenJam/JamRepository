using Core.Classes;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackManager : MonoBehaviour
{

    public static AttackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void GetSelectAttack(Vector2 startPosition, Vector2 direction, BulletInfo bulletInfo)
    {
        switch (bulletInfo.Type)
        {
            case BulletInfo.BulletType.line:
                GetLine(startPosition, direction, bulletInfo);
                break;
            case BulletInfo.BulletType.sin:
                break;
            case BulletInfo.BulletType.circle:
                GetCircle(startPosition, direction, bulletInfo);
                break;
            case BulletInfo.BulletType.fraction:
                GetFraction(startPosition, direction, bulletInfo);
                break;
            case BulletInfo.BulletType.firework:
                break;
            default:
                break;
        }
    }

    void GetLine(Vector2 startPosition, Vector2 direction, BulletInfo bulletInfo)
    {
        BulletPoolingManager.Instance.GetBullet(startPosition, direction, bulletInfo);
    }

    void GetCircle(Vector2 startPosition, Vector2 direction, BulletInfo bulletInfo)
    {
        if (bulletInfo.Coint == 0)
        {
            bulletInfo.Coint = 1;
        }
        if (bulletInfo.Radius == 0)
        {
            bulletInfo.Radius = 1;
        }
        float anlgeCoint = 360 / bulletInfo.Coint;
        float angle = 0;
        float positionX;
        float positionY;
        for (int i = 0; i < bulletInfo.Coint; i++)
        {
            positionX = startPosition.x + Mathf.Cos((angle * Mathf.PI) / 180) * bulletInfo.Radius;
            positionY = startPosition.y + Mathf.Sin((angle * Mathf.PI) / 180) * bulletInfo.Radius;
            Vector2 _direction = (new Vector2(positionX, positionY) - (Vector2)startPosition).normalized;
            BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletInfo);
            angle += anlgeCoint;
        }
    }

    void GetFraction(Vector2 startPosition, Vector2 direction, BulletInfo bulletInfo)
    {
        if (bulletInfo.Coint == 0)
        {
            bulletInfo.Coint = 1;
        }
        if (bulletInfo.Radius <= 5)
        {
            bulletInfo.Radius = 5;
        }
        float anlgeCoint = bulletInfo.Radius / bulletInfo.Coint;
        float angle = 0;
        float positionX;
        float positionY;
        Vector2 vec = (startPosition + direction) - Vector2.up;
        angle = Mathf.Atan2(direction.y - Vector2.up.y, direction.x - Vector2.up.x);
        //if (angle >= 0) { angle += anlgeCoint * (bulletInfo.Coint / 2); }
        //if (angle < 0) { angle -= anlgeCoint * (bulletInfo.Coint / 2); }
        //angle = Mathf.Atan2(Vector2.up.y - direction.y, Vector2.up.x- direction.x) * Mathf.Rad2Deg;
        // angle = Mathf.Atan2(direction.x - Vector2.up.x, direction.y - Vector2.up.y) * Mathf.Rad2Deg;
        for (int i = 0; i < bulletInfo.Coint; i++)
        {
            if(angle > 360) { angle -= 360;  }
            if(angle < 360) { angle += 360; }
            positionX = startPosition.x + Mathf.Cos(angle) + direction.x;
            positionY = startPosition.y + Mathf.Sin(angle) + direction.y;
            Vector2 _direction = (new Vector2(positionX, positionY) - (Vector2)startPosition).normalized;
            BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletInfo);
            //BulletPoolingManager.Instance.GetBullet(startPosition, new Vector2(positionX, positionY), bulletInfo);
            if (angle >= 0) { angle += anlgeCoint; }
            if (angle < 0) { angle -= anlgeCoint; }
            
        }
    }
}
