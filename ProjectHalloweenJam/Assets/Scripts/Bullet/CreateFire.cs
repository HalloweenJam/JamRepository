using System;
using UnityEngine;

namespace Bullet
{
    public class CreateFire : MonoBehaviour
    {
        public GameObject firePrefab;
        public float Speed;
        public int StartCount = 50;
        public int AddCount = 10;

        Transform bullet = null;
    
        private void Start()
        {
            CreatePrefab(StartCount);
        }

        public void Shooting(Vector3 startPosition, Vector3 direction)
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

                Continue:
                try
                {
                    bullet.gameObject.SetActive(true);
                    bullet.transform.position = startPosition;

                    //Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 aimDirection = (direction - bullet.position).normalized;
                    float rotateZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                    bullet.eulerAngles = new Vector3(0f, 0f, rotateZ);

                    bullet.GetComponent<Rigidbody2D>().velocity = bullet.right * Speed * Time.fixedDeltaTime;
                }
                catch (Exception)
                {
                    Debug.LogError("No Bullet");
                }
            }
        }

        private GameObject CreatePrefab(int Create)
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
}
