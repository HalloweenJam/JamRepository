using Core;
using Enemy.Arena;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private Castscene _castscene;
    private Arena _arena;

    private void Awake() => _arena = transform.root.GetComponent<Arena>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7) 
        {
            if (_arena.IsBossArena)
                _castscene.Activate();
            _arena.Activate();
            gameObject.SetActive(false);
        }
    }
}
