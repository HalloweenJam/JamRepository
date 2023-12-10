using UnityEngine;
using System.Collections.Generic;
using System;
using Enemy.EnemyEntity;
using Utilities.Classes;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Generation/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int _startPoints;
    [SerializeField] private WeightedRandomList<EnemyItem> _defaultEmemyItems = new ();
    [SerializeField] private WeightedRandomList<EnemyItem> _miniBossesEmemyItems = new();

    public int StartPoints => _startPoints;
    public IReadOnlyList<WeightedRandomList<EnemyItem>.Pair> DefaultEnemyItems => _defaultEmemyItems.weightedList;
    public IReadOnlyList<WeightedRandomList<EnemyItem>.Pair> MiniBossesEmemyItems => _miniBossesEmemyItems.weightedList;
    public EnemyItem GetDefaultEnemy() => _defaultEmemyItems.GetWeightedRandom();
    public EnemyItem GetMiniBoss() => _miniBossesEmemyItems.GetWeightedRandom();
}

[Serializable]
public class EnemyItem 
{
    public EnemyStats Prefab;
    public int Point;
}
