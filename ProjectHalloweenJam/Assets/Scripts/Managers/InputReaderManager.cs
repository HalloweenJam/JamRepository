using Player.Controls;
using UnityEngine;

namespace Managers
{
    public class InputReaderManager : PersistentSingleton<InputReaderManager>
    {
        [SerializeField] private InputReader _inputReader;

        public void Init() => base.Awake();
        
        public InputReader GetInputReader() => _inputReader;
    }
}