using Player;
using UnityEngine;
using NavMeshPlus.Components;
using Core;

namespace Managers
{
    public class Bootstrap : Singleton<Bootstrap>
    {
        [SerializeField] private InputReaderManager _inputReaderManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private bool _useStartProperties = false;

        [Header("StartGameProperty")]
        [SerializeField, HideInInspector] private Minimap _minimap;
        [SerializeField, HideInInspector] private BoxCollider2D _mainCollider;
        [SerializeField, HideInInspector] private BoxCollider2D _collisionCollider;
        [SerializeField, HideInInspector] private NavMeshSurface _navMeshSurface;
        [SerializeField, HideInInspector] private SpriteRenderer _fogOfWarSprite;

        public bool UseStartProperties => _useStartProperties;

        protected override void Awake()
        {
            base.Awake();
            _inputReaderManager.Init();
            _playerController.Init();
        }

        public void EnableComponents(int minimapRange)
        {
            _minimap.SetMinimap(minimapRange);

            _navMeshSurface.RemoveData();
            _navMeshSurface.AddData();
            var data = _navMeshSurface.navMeshData;
            _navMeshSurface.UpdateNavMesh(data);

            _mainCollider.enabled = true;
            _collisionCollider.enabled = true;
            _fogOfWarSprite.Activate();

            SceneTransition.Instance.OpenScene();
        }
    }
}