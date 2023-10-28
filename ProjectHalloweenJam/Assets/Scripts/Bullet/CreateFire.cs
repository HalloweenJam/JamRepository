using Player.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CreateFire : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private float Speed;
    [SerializeField] private int StartCount = 50;
    [SerializeField] private int AddCount = 10;

    Vector3 _worldMousePosition;

    

    [SerializeField]
    private List<BulletInfo> _bullets;


    Transform bullet = null;
    InputReader _inputReader;

    private void Start()
    {
        CreatePrefab(StartCount);
        /*_inputReader = InputReaderManager.Instance.GetInputReader();
        _inputReader.MousePosition += MousePosition;
        _inputReader.ShootingEvent += ShootingEvent;*/
    }

    /*private void ShootingEvent()
    {
        OnShooting(1, new Vector3(0, 0, 0), _worldMousePosition);
    }

    private void MousePosition(Vector2 vector)
    {
        _worldMousePosition = Camera.main.ScreenToWorldPoint(vector);
    }*/

    public void OnShooting(int index, Vector3 startPosition, Vector3 direction)
    {
        switch(_bullets[index].Type) 
        {
            case BulletInfo.BulletType.line:
                Shooting(_bullets[index], startPosition, direction);
                break;
            case BulletInfo.BulletType.sin:
                break;
            case BulletInfo.BulletType.circle:
                CircleShooting(_bullets[index], startPosition, direction);
                break;
            case BulletInfo.BulletType.fraction:
                break;
            case BulletInfo.BulletType.firework:
                break;
            default:
                break;
        }
    }


    void Shooting(BulletInfo bulletInfo,Vector3 startPosition, Vector3 direction)
    {
        if (firePrefab != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeSelf)
                {
                    bullet = transform.GetChild(i);
                    goto Continue;
                }

            }
            bullet = CreatePrefab(AddCount).transform;
            Shooting(bulletInfo, startPosition, direction);
        Continue:
            try
            {
                bullet.gameObject.SetActive(true);
                bullet.transform.position = startPosition;

                //Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 aimDirection = (direction - bullet.position).normalized;
                float rotateZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                bullet.eulerAngles = new Vector3(0f, 0f, rotateZ);
                bullet.GetComponent<BulletSys>().OnAttack(bulletInfo, BulletInfo.BulletType.line);
                //bullet.GetComponent<Rigidbody2D>().velocity = bullet.right * Speed * Time.fixedDeltaTime;
            }
            catch (System.Exception)
            {
                Debug.LogError("No Bullet");
            }
        }
    }

    void CircleShooting(BulletInfo bulletInfo, Vector3 startPosition, Vector3 direction)
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
            Shooting(bulletInfo, startPosition, new Vector3(positionX, positionY, 0));
            angle += anlgeCoint;
        }
    }

    GameObject CreatePrefab(int Create)
    {
        GameObject bullet = null;
        for (int i = 0; i < Create; i++)
        {
            bullet = Instantiate(firePrefab, new Vector3(0, 0, -100), new Quaternion(), this.transform);
            bullet.SetActive(false);
        }
        return bullet;
    }
}
