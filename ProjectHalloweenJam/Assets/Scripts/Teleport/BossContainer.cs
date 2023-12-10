using UnityEngine;
using Utilities.Classes;


[CreateAssetMenu(fileName = "BossContainer", menuName = "LevelSettings/BossContainer")]
public class BossContainer : ScriptableObject
{    
    [SerializeField] private WeightedRandomList<string> _bossSceneNames = new();

    public void LoadBossScene()
    {
        var name = _bossSceneNames.GetWeightedRandom();
        SceneTransition.SwitchToScene(name);
    }
}
