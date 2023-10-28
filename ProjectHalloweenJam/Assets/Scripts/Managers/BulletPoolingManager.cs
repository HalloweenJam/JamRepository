using Projectiles;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
    public class BulletPoolingManager : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;
        
        public static BulletPoolingManager Instance { get; private set; }

        private ObjectPool<Bullet> _bulletsPool;

        public void GetBullet(Vector2 startPosition, Vector2 direction, float speed, int damage)
        {
            var instance = _bulletsPool.Get();
            instance.transform.position = startPosition;
            instance.gameObject.SetActive(true);
            instance.Init(direction, speed, damage);
        }

        public void Release(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _bulletsPool.Release(bullet);
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
            _bulletsPool = new ObjectPool<Bullet>(CreateBullet);
        }

        private Bullet CreateBullet()
        {
            return Instantiate(_bullet, transform);
        }
    }
}