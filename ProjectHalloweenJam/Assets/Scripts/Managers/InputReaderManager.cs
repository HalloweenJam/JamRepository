﻿using Player.Controls;
using UnityEngine;

namespace Managers
{
    public class InputReaderManager : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        public static InputReaderManager Instance { get; private set; }

        public InputReader GetInputReader() => _inputReader;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}