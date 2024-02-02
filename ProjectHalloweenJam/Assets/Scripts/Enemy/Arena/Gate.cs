using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DissolveEffect))] 
public class Gate : MonoBehaviour
{
    [SerializeField, HideInInspector] private DissolveEffect _dissolveEffect;
    [SerializeField] private Collider2D _triggerCollider;
    private bool _isActivatable;
    public bool IsActivatable => _isActivatable;

    private void OnValidate() => _dissolveEffect = GetComponent<DissolveEffect>();

    private void Start() => _isActivatable = gameObject.activeInHierarchy;

    public void Disable() => StartCoroutine(Disabled());

    public void Enable()
    {
        gameObject.SetActive(true);
        _dissolveEffect.Appearance();
    }
 
    private IEnumerator Disabled()
    {
        _dissolveEffect.Dissolve();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7)
        {
            _triggerCollider.enabled = false;
            Disable();
        }
    }
}
