using Core;
using Generation.AnchorSystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Level = LevelSetting.Levels;

public partial class Arena : Room
{
    [SerializeField] private Level _level;
    [SerializeField] private SpawnPointsData _spawnPointsData;
    [SerializeField] private ActivateRoomZone _spawnZone;
    [SerializeField] private List<Gate> _gates;
    [SerializeField] private bool _isBossArena = false;

    [FormerlySerializedAs("_castscene")] [SerializeField, HideInInspector] private Cutscene _cutscene;
    private bool _isCompleted = false;

    private SpawnPointsData _thisSpawnPointsData;
    public bool IsBossArena => _isBossArena;
    public bool IsCompleted => _isCompleted;

    private void Awake() => _thisSpawnPointsData = _spawnPointsData;

    public override void Activate(bool useFow)
    {
        SpawnerEnemy.Instance?.ActivateArena(this, _level, _thisSpawnPointsData);
        foreach (Gate gate in _gates)
        {
            if(gate.IsActivatable || IsBossArena) 
                gate?.Enable();
        }

        if (IsBossArena)
            _cutscene.Activate();

        base.Activate(!_isBossArena);
    }

    public void Completed()
    {
        _isCompleted = true;
        foreach (Gate gate in _gates)
        {
            if (gate.gameObject.activeInHierarchy)
                gate.Disable();
        }
    }
}
