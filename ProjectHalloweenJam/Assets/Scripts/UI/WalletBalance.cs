using System;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WalletBalance : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _walletText;
        [SerializeField] private PlayerStats _playerStats;

        private void OnValidate()
        {
            _playerStats = FindAnyObjectByType<PlayerStats>();
        }

        private void Awake() => _playerStats.Wallet.WalletBalanceChangedAction += Change;
        private void OnDisable() =>  _playerStats.Wallet.WalletBalanceChangedAction -= Change;
        
        private void Change(int balance) => _walletText.text = $"Coins: {balance}";
    }
}