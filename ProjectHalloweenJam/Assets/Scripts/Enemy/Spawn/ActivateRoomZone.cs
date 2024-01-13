using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ActivateRoomZone : MonoBehaviour
{
    private Room _room;

    private void Awake() => _room = transform.root.GetComponent<Room>();  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7) 
        {
            _room.Activate(true);
            gameObject.SetActive(false);
        }
    }
}
