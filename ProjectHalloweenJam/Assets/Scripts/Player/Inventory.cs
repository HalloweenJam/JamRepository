using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _keysCount = 1;
        [SerializeField] private int _keysVariables = 5;
        
        private Dictionary<int, int> _keys = new();

        public Dictionary<int, int> Keys => _keys;
        
        private void Start()
        {
            for (int i = 1; i <= _keysVariables; i++)
            {
                _keys.Add(i, _keysCount);
            }
        }
    }
}