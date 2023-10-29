using Bullet;
using Core.Enums;
using UnityEngine;

namespace Managers
{
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
        public static void SelectAttack(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            switch (bulletConfig.Type)
            {
                case BulletType.Line:
                    GetLine(startPosition, direction, bulletConfig);
                    break;
                case BulletType.Sin:
                    break;
                case BulletType.Circle:
                    GetCircle(startPosition, direction, bulletConfig);
                    break;
                case BulletType.Fraction:
                    GetFraction(startPosition, direction, bulletConfig);
                    break;
                case BulletType.Firework:
                    break;
                default:
                    break;
            }
        }

        private static void GetLine(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            BulletPoolingManager.Instance.GetBullet(startPosition, direction, bulletConfig);
        }

        private static void GetCircle(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            var bulletCount = bulletConfig.Count == 0 ? 1 : bulletConfig.Count;
            var bulletRadius = bulletConfig.Radius == 0 ? 1 : bulletConfig.Radius; 
            
            float angleCount = 360 / bulletCount;
            float angle = 0;
            for (int i = 0; i < bulletConfig.Count; i++)
            {
                var positionX = startPosition.x + Mathf.Cos((angle * Mathf.PI) / 180) * bulletRadius;
                var positionY = startPosition.y + Mathf.Sin((angle * Mathf.PI) / 180) * bulletRadius;
                var _direction = (new Vector2(positionX, positionY) - startPosition).normalized;
                
                BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletConfig);
                angle += angleCount;
            }
        }

        private static void GetFraction(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            var bulletCount = bulletConfig.Count == 0 ? 1 : bulletConfig.Count;
            var bulletRadius = bulletConfig.Radius <= 5 ? 5 : bulletConfig.Radius;

            var angleStep = bulletRadius / (bulletCount - 1);
            var startAngle = -(bulletRadius / 2);

            var targetDirection = (startPosition + direction * 2) - startPosition;
            var targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            for (int i = 0; i < bulletCount; i++)
            {
                var angle = startAngle + i * angleStep; 
                var positionX = startPosition.x + Mathf.Cos((targetAngle + angle) * Mathf.PI / 180) * bulletRadius;
                var positionY = startPosition.y + Mathf.Sin((targetAngle + angle) * Mathf.PI / 180) * bulletRadius;
                var _direction = (new Vector2(positionX, positionY) - startPosition).normalized;
                
                BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletConfig);
            }
        }
    }
}
