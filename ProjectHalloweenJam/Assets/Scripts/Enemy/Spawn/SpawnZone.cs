using Core;
using System;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private bool _isBossArena = false;
    [SerializeField] private Castscene _castscene;

    public Action OnActivateArena;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7) 
        {
            if (_isBossArena)
                _castscene.Activate();
            OnActivateArena?.Invoke();
            this.Deactivate();
        }
    }
}
