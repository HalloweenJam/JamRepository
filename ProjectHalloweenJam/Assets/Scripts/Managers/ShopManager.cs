using System.Collections.Generic;
using Core.Classes;
using Items;
using UnityEngine;

namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        [SerializeField] private WeightedRandomList<List<Collectable>> _collectables;

        public Collectable GetCollectable()
        {
            var rankList = _collectables.GetWeightedRandom();
            var collectableIndex = Random.Range(0, rankList.Count);

            return rankList[collectableIndex];
        }
    }
}