using Core;
using Enemy.Arena;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private Arena _arena;

    private void Start() => _arena = transform.root.GetComponent<Arena>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7) // 7 is player layer
        {
            _arena.ActivateArena();
            this.Deactivate();
        }
    }
}
