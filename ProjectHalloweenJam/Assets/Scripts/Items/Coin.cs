using Player;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "coin", menuName = "Items/Coin", order = 0)]
    public class Coin : Collectable
    {
        [Space(20)]
        [SerializeField] private int _value;
        
        public override void Apply(PlayerStats playerStats)
        {
            PlayerStats.DepositMoney(_value);
        }
    }
}