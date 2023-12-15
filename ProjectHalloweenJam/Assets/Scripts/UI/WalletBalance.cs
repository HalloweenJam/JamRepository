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

        private void Awake() => PlayerStats.Wallet.WalletBalanceChangedAction += Change;
        private void OnDisable() =>  PlayerStats.Wallet.WalletBalanceChangedAction -= Change;
        
        private void Change(int balance) => _walletText.text = $"Coins: {balance}";
    }
}