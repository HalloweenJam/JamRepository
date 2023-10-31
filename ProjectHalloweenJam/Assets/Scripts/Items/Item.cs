using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "refill", menuName = "Items/Refill", order = 0)]
    public class Item : ScriptableObject
    {
        [SerializeField, Range(1f, 100f)] private float _refillPercent;
        [SerializeField] private bool _isPercent = true;
        [Space]
        [SerializeField] private Sprite _sprite;

        public float RefillPercent => _refillPercent;
        public bool IsPercent => _isPercent;
        public Sprite Sprite => _sprite;
    }
}