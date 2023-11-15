using Player;
using UnityEngine;

namespace Items
{
    public abstract class Collectable : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        
        [SerializeField] private int _cost;

        public string Name => _name;
        public int Cost => _cost;
        
        public Sprite Sprite
        {
            get => _sprite;
            protected set => _sprite = value;
        }

        public abstract void Apply(PlayerStats playerStats);
    }
}