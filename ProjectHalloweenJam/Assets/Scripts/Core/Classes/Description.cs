using System;
using UnityEngine;

namespace Core.Classes
{
    [Serializable]
    public class Description
    {
        [SerializeField] private Sprite _inHands;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _info;

        public Sprite InHands => _inHands;
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Info => _info;
    }
}