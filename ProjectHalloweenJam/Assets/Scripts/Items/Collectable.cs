using Player;
using UnityEngine;

namespace Items
{
    public abstract class Collectable : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;

        public abstract void Apply(PlayerStats playerStats);
    }
}