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

        public PlayerStats PlayerStats { get; private set; }

        public Dictionary<int, int> Keys { get; } = new();

        private void OnValidate()
        {
            PlayerStats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            for (int i = 1; i <= _keysVariables; i++)
            {
                Keys.Add(i, _keysCount);
            }
        }
    }
}