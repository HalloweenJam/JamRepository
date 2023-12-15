using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlowEffect : MonoBehaviour
{
    [SerializeField] private Material _glowMaterial;
    private SpriteRenderer _spriteRenderer;
    private Material _currentMaterial;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentMaterial = _spriteRenderer.material;
    }

    public void ActivateGlow() => _spriteRenderer.material = _glowMaterial ;
    public void DeactivateGlow() => _spriteRenderer.material = _currentMaterial;
} 
