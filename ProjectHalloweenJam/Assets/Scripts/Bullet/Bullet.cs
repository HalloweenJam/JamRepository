using Core.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageable
{
    [SerializeField] private int _damage;

    private float _timeHeart;
    private float _timerHeart;
    private float _speed;

    Rigidbody2D _rg;
    SpriteRenderer _spriteRenderer;
    CircleCollider2D _circleCollider;

    void Awake()
    {
        _rg = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public bool TryTakeDamage(int damage, bool instantKill = false, bool ignoreInvisibility = false)
    {
        //throw new System.NotImplementedException();
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TryTakeDamage(_damage);
        }
        if (other.tag == "Wall") 
        {
            transform.position = new Vector3(0, 0, -100);
            this.gameObject.SetActive(false);
        }
        
    }

    public void OnAttack(BulletInfo info, BulletInfo.BulletType type)
    {
        _spriteRenderer.sprite = info.Sprite;
        _circleCollider.radius = info.Size;
        _speed = info.Speed;
        _timeHeart = _timerHeart= info.TimeHeart;
        _damage = info.Damage;
        switch (type)
        {
            case BulletInfo.BulletType.line:
                StartCoroutine(Line());
                break;
            case BulletInfo.BulletType.sin:
                break;
            default: break;
        }
    }

    private IEnumerator Line()
    {
        while (_timerHeart > 0)
        {
            _rg.velocity = transform.right * _speed * Time.fixedDeltaTime;
            _timerHeart -= 1;
            yield return null;
        }
        Dead();
    }

    void Dead()
    {
        transform.position = new Vector3(0, 0, -100);
        this.gameObject.SetActive(false);
        _timerHeart = _timeHeart;
    }
}
