using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DissolveEffect))]
public class Gate : MonoBehaviour
{
    private DissolveEffect _dissolveEffect;

    private void Start()
    {
        _dissolveEffect = GetComponent<DissolveEffect>();
        gameObject.SetActive(false);
    }

    public void OnEnable() => _dissolveEffect?.Appearance(null);

    public void Disable() =>  StartCoroutine(Disabled());
 
    private IEnumerator Disabled()
    {
        _dissolveEffect.Dissolve(null);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
