using Player;
using UnityEngine;

namespace Managers
{
    public class Bootstrap : Singleton<Bootstrap>
    {
        [SerializeField] private InputReaderManager _inputReaderManager;
        [SerializeField] private PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();
            _inputReaderManager.Init();
            _playerController.Init();
        }
    }
}