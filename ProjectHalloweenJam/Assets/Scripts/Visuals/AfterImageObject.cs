using UnityEngine;

namespace Visuals
{
    public class AfterImageObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _playerRenderer;
        [SerializeField] private SpriteRenderer _childRenderer;

        public SpriteRenderer ChildRenderer => _childRenderer;
        public SpriteRenderer PlayerRenderer => _playerRenderer;

        public float DecreaseAlpha(float alpha)
        {
            var spriteRendererColor = _childRenderer.color;
            
            spriteRendererColor.a -= alpha;
            _childRenderer.color = spriteRendererColor;
            _playerRenderer.color = spriteRendererColor;

            return spriteRendererColor.a;
        }
    }
}