using System;
using Player.Controls;
using UnityEngine;

namespace Managers
{
    public class InputReaderManager : Singleton<InputReaderManager>
    {
        [SerializeField] private InputReader _inputReader;

        public Action<bool> OnInputReaderActiveStateChanged;
        
        public void Init() => base.Awake();
        
        public InputReader GetInputReader() => _inputReader;

        public void SetActiveControls(bool active)
        {
            if (!active)
            {
                _inputReader.SetUIActions();
            }
            else
            {
                _inputReader.SetPlayerActions();
            }
            
            OnInputReaderActiveStateChanged?.Invoke(active);
        }
    }
}