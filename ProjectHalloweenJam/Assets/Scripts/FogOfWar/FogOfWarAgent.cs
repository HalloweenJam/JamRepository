using System.Collections;
using UnityEngine;

public class FogOfWarAgent : MonoBehaviour
{ 
    [SerializeField] private Transform _secondaryFogOfWar;
    [Space]
    [SerializeField]
    [Range(0.0f, 5.0f)] private float _timeInterval;
    [SerializeField] private float _sightDistance;
    private Coroutine _fowCoroutine;

    private void Start()
    {
        _fowCoroutine = StartCoroutine(CheckFogOfWar(_timeInterval));
        _secondaryFogOfWar.localScale = new Vector2(_sightDistance, _sightDistance) * 10f;
    }    

    private IEnumerator CheckFogOfWar(float checkInterval)
    {
        while (true)
        {
            FogOfWar.Instance.MakeHole(transform.position, _sightDistance);
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(checkInterval * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        if (_fowCoroutine != null)
            StopCoroutine(_fowCoroutine);
    }
}
