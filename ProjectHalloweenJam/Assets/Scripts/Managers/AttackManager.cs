using Bullet;
using Core.Classes;
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
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }
        public void GetSelectAttack(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            switch (bulletConfig.Type)
            {
                case BulletConfig.BulletType.line:
                    GetLine(startPosition, direction, bulletConfig);
                    break;
                case BulletConfig.BulletType.sin:
                    break;
                case BulletConfig.BulletType.circle:
                    GetCircle(startPosition, direction, bulletConfig);
                    break;
                case BulletConfig.BulletType.fraction:
                    GetFraction(startPosition, direction, bulletConfig);
                    break;
                case BulletConfig.BulletType.firework:
                    break;
                default:
                    break;
            }
        }

        void GetLine(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            BulletPoolingManager.Instance.GetBullet(startPosition, direction, bulletConfig);
        }

        void GetCircle(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            if (bulletConfig.Coint == 0)
            {
                bulletConfig.Coint = 1;
            }
            if (bulletConfig.Radius == 0)
            {
                bulletConfig.Radius = 1;
            }
            float anlgeCoint = 360 / bulletConfig.Coint;
            float angle = 0;
            float positionX;
            float positionY;
            for (int i = 0; i < bulletConfig.Coint; i++)
            {
                positionX = startPosition.x + Mathf.Cos((angle * Mathf.PI) / 180) * bulletConfig.Radius;
                positionY = startPosition.y + Mathf.Sin((angle * Mathf.PI) / 180) * bulletConfig.Radius;
                Vector2 _direction = (new Vector2(positionX, positionY) - (Vector2)startPosition).normalized;
                BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletConfig);
                angle += anlgeCoint;
            }
        }

        void GetFraction(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            if (bulletConfig.Coint == 0)
            {
                bulletConfig.Coint = 1;
            }
            if (bulletConfig.Radius <= 5)
            {
                bulletConfig.Radius = 5;
            }
            float anlgeCoint = bulletConfig.Radius / bulletConfig.Coint;
            float angle = 0;
            float positionX;
            float positionY;
            Vector2 vec = (startPosition + direction) - Vector2.up;
            angle = Mathf.Atan2(direction.y - Vector2.up.y, direction.x - Vector2.up.x) * Mathf.Rad2Deg;
            for (int i = 0; i < bulletConfig.Coint; i++)
            {
                positionX = startPosition.x + Mathf.Cos(angle) + direction.x;
                positionY = startPosition.y + Mathf.Sin(angle) + direction.y;
                Vector2 _direction = (new Vector2(positionX, positionY) - (Vector2)startPosition).normalized;
                BulletPoolingManager.Instance.GetBullet(startPosition, _direction, bulletConfig);
                angle += anlgeCoint;
            }
        }
    }
}
