using Bullet;
using Core.Classes;
using Projectiles;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
    public class BulletPoolingManager : MonoBehaviour
    {
        [SerializeField] private Projectiles.Bullet _bullet;
        
        public static BulletPoolingManager Instance { get; private set; }

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
            try {_bulletsPool.Release(bullet); } catch { }
        }

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

        private void Start()
        {
            _bulletsPool = new ObjectPool<Projectiles.Bullet>(CreateBullet);
        }

        private Projectiles.Bullet CreateBullet()
        {
            return Instantiate(_bullet, transform);
        }
    }
}