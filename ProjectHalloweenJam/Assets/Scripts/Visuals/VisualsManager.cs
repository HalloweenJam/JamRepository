using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Visuals
{
    public class VisualsManager : Singleton<VisualsManager>
    {
        [SerializeField] private AfterImageObject _spriteRendererHolder;

        private readonly List<AfterImageObject> _afterImageObjects = new();
        private ObjectPool<AfterImageObject> _spritePool;

        public void CreateAfterImage(SpriteRenderer playerRenderer, SpriteRenderer childRenderer, float startOpacity)
        {
            var afterImageObject = _spritePool.Get();
            var child = afterImageObject.ChildRenderer;
            var player = afterImageObject.PlayerRenderer;
            
            afterImageObject.gameObject.layer = playerRenderer.gameObject.layer;
            
            CopyRenderer(playerRenderer, player, startOpacity, 10);
            CopyRenderer(childRenderer, child, startOpacity, 12);
            
            afterImageObject.gameObject.SetActive(true);
            
            AddToOpacityQueue(afterImageObject);
        }

        private static void CopyRenderer(SpriteRenderer toCopy, SpriteRenderer result, float startOpacity, int sortingOrder)
        {
            var copyTransform = toCopy.transform;
            var resultTransform = result.transform;
            
            result.sprite = toCopy.sprite;
            result.sortingLayerID = toCopy.sortingLayerID;
            result.sortingLayerName = toCopy.sortingLayerName;
            result.sortingOrder = toCopy.sortingOrder - sortingOrder;
            result.flipX = toCopy.flipX;
            result.flipY = toCopy.flipY;

            resultTransform.rotation = copyTransform.rotation;
            resultTransform.position = copyTransform.position;
            resultTransform.localScale = copyTransform.localScale;

            var color = result.color;
            color.a = startOpacity;
            result.color = color;
        }

        private void AddToOpacityQueue(AfterImageObject spriteRenderer)
        {
            _afterImageObjects.Add(spriteRenderer);
        }

        private void Start()
        {
            _spritePool = new ObjectPool<AfterImageObject>(CreateSprite);
        }

        private AfterImageObject CreateSprite() => Instantiate(_spriteRendererHolder, transform);

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            var toDestroy =
                _afterImageObjects.Where(afterImageObject => !(afterImageObject.DecreaseAlpha(deltaTime * .5f) > 0));  

            foreach (var spriteRenderer in toDestroy.ToList())
            {
                spriteRenderer.gameObject.SetActive(false);
                
                _afterImageObjects.Remove(spriteRenderer);
                _spritePool.Release(spriteRenderer);
            }
        }
    }
}