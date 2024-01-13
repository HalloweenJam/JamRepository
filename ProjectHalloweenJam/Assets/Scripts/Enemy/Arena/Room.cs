using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("FogOfWar")]
    [SerializeField] private Transform _centerRoom;
    [SerializeField] private float _radius;

    public virtual void Activate(bool useFow)
    {
        if(useFow)
            FogOfWar.Instance?.MakeHole(_centerRoom.position, _radius);
    } 
}
