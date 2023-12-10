using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "LevelSettings/LevelConfiguration")]
public class LevelConfiguration : ScriptableObject
{
    [SerializeField] private List<LevelSetting> _levelSettings = new();
    private int _wavesCount;

    public (int level, LevelSetting setting) GetSettings(LevelSetting.Levels level)
    {
        LevelSetting result = default;
        foreach (LevelSetting setting in _levelSettings)
        {
            if(setting.Level == level)
                result = setting;
        }
        int intLevel = (int)result.Level + 1;
        return (intLevel, result);
    }

    public (int waves, int maxEnemyCount, bool canSpawnMiniBoss) SetRandomValue(int arenaLevel, LevelSetting settings)
    {
        System.Random random = new();
        _wavesCount = random.Next(arenaLevel, arenaLevel + 1);
        var enemyInScene = random.Next(settings.MinEnemyInScene, settings.MaxEnemyInScene);

        var ramdomWeight = random.Next(1, 100);
        bool canSpawnMiniBoss = ramdomWeight <= settings.MiniBossWeight;
        return (_wavesCount, enemyInScene, canSpawnMiniBoss);
    }

    private void OnValidate()
    {
        foreach (LevelSetting settings in _levelSettings)
        {
            if (settings.MinEnemyInScene > settings.MaxEnemyInScene)
                settings.SetMaxValueInEditor(settings.MinEnemyInScene);
        }
    }
}

[Serializable]
public class LevelSetting
{ 
    public enum Levels 
    { 
        Level_1,
        Level_2,
        Level_3,
        Level_4
    }               

    [Range(2, 8)] [SerializeField] private int _minEnemyInScene;
    [Range(2, 8)] [SerializeField] private int _maxEnemyInScene;
    [SerializeField] private int _miniBossWeight;
    [SerializeField] private Levels _level;
                     
    public int MinEnemyInScene => _minEnemyInScene;
    public int MaxEnemyInScene => _maxEnemyInScene;
    public int MiniBossWeight => _miniBossWeight;
    public Levels Level => _level;

    public void SetMaxValueInEditor(int maxValue) => _maxEnemyInScene = maxValue;
}   
