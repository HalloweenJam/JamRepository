using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DissolveEffect))] 
public class Gate : MonoBehaviour
{
    [SerializeField] private Collider2D _triggerCollider;
    private DissolveEffect _dissolveEffect;
    private bool _isActivatable;
    public bool IsActivatable => _isActivatable;

    private void Start()
    {
        _dissolveEffect = GetComponent<DissolveEffect>();
        _isActivatable = gameObject.activeInHierarchy;
    }

    public void Enable()
    {
        gameObject.SetActive(true);  
        _dissolveEffect.Appearance();
    }

    public void Disable() =>  StartCoroutine(Disabled());
 
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
