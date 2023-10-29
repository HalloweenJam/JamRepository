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
            Destroy(gameObject);
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
        Vector2 targetDirection = (startPosition + direction * 2) - startPosition;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        
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
        float positionX;
        float positionY;
        int numberOfBullets = bulletInfo.Coint; // Количество пуль в полукруге
        float angleStep = bulletInfo.Radius / (numberOfBullets - 1); // Шаг угла между пулями
        float startAngle = -(bulletInfo.Radius / 2); // Начальный угол

        Vector2 targetDirection = (startPosition + direction * 2) - startPosition;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + i * angleStep; // Вычисляем угол для каждой пули
            positionX = startPosition.x + Mathf.Cos(((targetAngle + angle) * Mathf.PI) / 180) * bulletInfo.Radius;
            positionY = startPosition.y + Mathf.Sin(((targetAngle + angle) * Mathf.PI) / 180) * bulletInfo.Radius;
            Vector2 _direction = (new Vector2(positionX, positionY) - (Vector2)startPosition).normalized;
            BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletInfo);
        }
    }
}
