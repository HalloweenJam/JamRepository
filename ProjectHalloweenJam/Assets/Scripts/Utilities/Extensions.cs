using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Utilities
{
    public static class Extensions
    {
        public static List<T> Join<T, SList>(this SList first, SList second) where SList : IReadOnlyList<T>
        {
            if (first == null)
                return second as List<T>;
            
            if (second == null) 
                return first as List<T>;
 
            return  first.Concat(second).ToList();
        }
        
        public static int GetRandomUnusedId<T>(this IReadOnlyList<T> list, List<int> usedIds)
        {
            if (usedIds.Count == list.Count)
                throw new Exception("ID selection is not possible, since the number of items in the transmitted list is equal to the number of used IDs");
            
            var index = GetRandomId(list);

            while (usedIds.Contains(index))
            {
                index = GetRandomId(list);
            }

            return index;
        }
        
        public static int GetRandomId<T>(this IReadOnlyList<T> list)
        {
            return Random.Range(0, list.Count);
        }
    }
}