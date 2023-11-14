using Player;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "refill", menuName = "Items/Refill", order = 0)]
    public class Refill : Collectable
    {
        [Space(20)]
        [SerializeField, Range(1f, 100f)] private float _refillAmount;
        [SerializeField] private bool _isPercent = true;
        
        public override void Apply(PlayerStats playerStats)
        {
            playerStats.WeaponSelector.AddRefill(_refillAmount, _isPercent);
        }
    }
}