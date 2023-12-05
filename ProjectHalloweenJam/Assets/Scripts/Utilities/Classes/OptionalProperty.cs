using System;
using UnityEngine;

namespace Utilities.Classes
{
    [Serializable]
    public class OptionalProperty<T>
    {
        [SerializeField] private T _value;
        [SerializeField] private bool _enabled;
        
        public T Value => _value;
        public bool Enabled => _enabled;
        
        public OptionalProperty(T initialValue)
        {
            _enabled = true;
            _value = initialValue;
        }
    }
}