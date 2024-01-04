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

        [SerializeField] private Minimap _minimap;
        [SerializeField] private BoxCollider2D _mainCollider;
        [SerializeField] private BoxCollider2D _collisionCollider;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private SpriteRenderer _fogOfWarSprite;


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
        }
    }
}