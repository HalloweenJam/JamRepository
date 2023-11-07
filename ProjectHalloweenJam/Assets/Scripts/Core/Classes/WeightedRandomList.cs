using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Core.Classes
{
    [Serializable]
    public class WeightedRandomList<T>
    {
        [Serializable]
        public struct Pair
        {
            public T weightedItem;
            public float weight;

            public Pair(T weightedItem, float weight)
            {
                this.weightedItem = weightedItem;
                this.weight = weight;
            }
        }

        public List<Pair> weightedList = new();

        public int Count => weightedList.Count;

        public void Add(T item, float weight)
        {
            weightedList.Add(new Pair(item, weight));
        }

        public T GetWeightedRandom()
        {
            var totalWeight = weightedList.Sum(p => p.weight);

            var value = Random.value * totalWeight;

            float sumWeight = 0;

            foreach (var p in weightedList)
            {
                sumWeight += p.weight;

                if (sumWeight >= value)
                {
                    return p.weightedItem;
                }
            }
            return default;
        }
    }
}