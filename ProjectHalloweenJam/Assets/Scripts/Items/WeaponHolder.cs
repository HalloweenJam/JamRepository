using Player;
using UnityEngine;
using Weapons;

namespace Items
{
    [CreateAssetMenu(fileName = "weapon holder", menuName = "Items/Weapon Holder", order = 0)]
    public class WeaponHolder : Collectable
    {
        [Space(20)]
        [SerializeField] private BaseWeapon _weapon;

        private void OnValidate()
        {
            if (_weapon != null)
                Sprite = _weapon.Description.Icon;
        }

        public override void Apply(PlayerStats playerStats)
        {
            playerStats.WeaponSelector.Add(_weapon);
        }
    }
}