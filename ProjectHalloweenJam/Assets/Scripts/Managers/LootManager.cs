using System.Collections.Generic;
using Core.Classes;
using Enemy.EnemyEntity;
using Gameplay.Interactions;
using Items;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class LootManager : MonoBehaviour
    {
        [SerializeField] private ItemHolder _itemHolder;
        [Space]
        [SerializeField] private WeightedRandomList<List<Item>> _weightedLootList = new();

        private void Awake() => EnemyStats.OnDeath += SpawnLoot;

        private void OnDisable() => EnemyStats.OnDeath -= SpawnLoot;
        
        private void SpawnLoot(Vector2 position, float dropChance)
        {
            var chance = Random.value;

            if (dropChance < chance)
                return;
            
            var rankList = _weightedLootList.GetWeightedRandom();
            var lootIndex = Random.Range(0, rankList.Count);

            var item = rankList[lootIndex];

            var itemHolder = Instantiate(_itemHolder, position, quaternion.identity);
            itemHolder.SetItem(item);
        }
    }
}