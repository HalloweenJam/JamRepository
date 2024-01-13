using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [Header("Dissolve")]
    [SerializeField] private float _dissolveTime = 1f;
    [SerializeField] private Material _dissolveMaterial;
    [SerializeField] private bool _activateComponent = true;

    private SpriteRenderer _spriteRenderer;

    private static readonly int Fade = Shader.PropertyToID("_Fade");

    private bool _dissolved = false;
    private float _elapsedTime = 0f;

    public bool Dissolved => _dissolved;
    public Material DissolveMaterial => _dissolveMaterial;
    public bool CanActivateComponent => _activateComponent;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = _dissolveMaterial;
    }

    public void Appearance(params Behaviour[] components) 
        => StartCoroutine(DissolveCoroutine(true, components));

    public void Dissolve(params Behaviour[] components)
        => StartCoroutine(DissolveCoroutine(false, components));

    private IEnumerator DissolveCoroutine(bool isAppearance, params Behaviour[] components)
    {
        _dissolved = false;
        float fadeMaterial = isAppearance ? 0 : 1f;
        float fadeAbs = isAppearance ? 1f : 0f;
        float percent = isAppearance ? (1 / _dissolveTime) : -(1 / _dissolveTime); 

        while (_elapsedTime < _dissolveTime)
        {
            fadeMaterial += percent * Time.deltaTime;
            _elapsedTime += Time.deltaTime;
            _spriteRenderer.material.SetFloat(Fade, fadeMaterial);
            yield return null;
        }
        _elapsedTime = 0f;

        if (components != null)
        {
            foreach (Behaviour component in components)   
                if(component != null)
                    component.enabled = true;            
        }

        _spriteRenderer.material.SetFloat(Fade, fadeAbs);
        _dissolved = true;
    }
}
