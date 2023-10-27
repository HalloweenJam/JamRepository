using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall") 
        {
            transform.position = new Vector3(0, 0, -100);
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.gameObject.SetActive(false);
        }
    }
}
