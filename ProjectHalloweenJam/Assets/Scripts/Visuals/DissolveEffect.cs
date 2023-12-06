using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [Header("Dissolve")]
    [SerializeField] private float _dissolveTime = 1f;
    [SerializeField] private Material _dissolveMaterial;
    [SerializeField] private bool _canAppearance = true;

    private SpriteRenderer _spriteRenderer;

    private static readonly int Fade = Shader.PropertyToID("_Fade");

    private bool _dissolved = false;
    private float _elapsedTime = 0f;

    public bool Dissolved => _dissolved;
    public Material DissolveMaterial => _dissolveMaterial;
    public bool CanAppearance => _canAppearance;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = _dissolveMaterial;
    }

    public void Appearance(Behaviour component) 
        => StartCoroutine(AppearanceCor(component));

    public void Dissolve(Behaviour component)
        => StartCoroutine(DissolveCor(component));

    private IEnumerator AppearanceCor(Behaviour component)
    {
        _dissolved = false;
        float fadeMaterial = 0;
        float percent = (1 / _dissolveTime);

        while (_elapsedTime < _dissolveTime)
        {
            fadeMaterial += percent * Time.deltaTime;
            _elapsedTime += Time.deltaTime;
            _spriteRenderer.material.SetFloat(Fade, fadeMaterial);
            yield return null;
        }
        _elapsedTime = 0f;
        if(component != null)
            component.enabled = true;
        _spriteRenderer.material.SetFloat(Fade, 1f);
        _dissolved = true;
    }

    private IEnumerator DissolveCor(Behaviour component)
    {
        _dissolved = false;
        float fadeMaterial = 1;
        float percent = -(1 / _dissolveTime);

        while (_elapsedTime < _dissolveTime)
        {
            fadeMaterial += percent * Time.deltaTime;
            _elapsedTime += Time.deltaTime;
            _spriteRenderer.material.SetFloat(Fade, fadeMaterial);
            yield return null;
        }
        _elapsedTime = 0f;
        if (component != null)
            component.enabled = true;
        _spriteRenderer.material.SetFloat(Fade, 0f);
        _dissolved = true;
    }
}
