using System;
using Bullet;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
    public class BulletPoolingManager : Singleton<BulletPoolingManager>
    {
        [SerializeField] private Projectiles.Bullet _bullet;
        
        private ObjectPool<Projectiles.Bullet> _bulletsPool;

        public void GetBullet(Vector2 startPosition, Vector2 direction, BulletConfig bulletConfig)
        {
            var instance = _bulletsPool.Get();
            instance.transform.position = startPosition;
            instance.gameObject.SetActive(true);
            instance.Init(direction, bulletConfig);
        }

        public void Release(Projectiles.Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            try
            {
                _bulletsPool.Release(bullet);
            }
            catch { }
        }
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _bulletsPool = new ObjectPool<Projectiles.Bullet>(CreateBullet);
        }

        private Projectiles.Bullet CreateBullet()
        {
            return Instantiate(_bullet, transform);
        }
    }
}