using System;
using Core.Interfaces;
using UnityEngine;

namespace Gameplay.Interactions
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _rarity = 1;


        public bool Interact()
        {
            throw new NotImplementedException();
        }
    }
}