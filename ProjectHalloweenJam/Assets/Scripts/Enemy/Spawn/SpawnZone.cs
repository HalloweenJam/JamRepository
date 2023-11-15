using Core;
using Enemy.Arena;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private bool _isBossArena = false;
    [SerializeField] private Castscene castscene;
    private Arena _arena;

    private void Start() => _arena = transform.root.GetComponent<Arena>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7) // 7 is player layer
        {
            if (_isBossArena)
                castscene.Activate();
            _arena.ActivateArena();
            this.Deactivate();
        }
    }
}
