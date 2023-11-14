using System;

namespace Core.Classes
{
    public class Wallet
    {
        private int _handledPoints = 0;
        
        public Action<int> WalletBalanceChangedAction;
        
        public void DepositPoints(int points)
        {
            if (points <= 0)
                throw new Exception("money amount cannot be less than 0");

            _handledPoints += points;
            WalletBalanceChangedAction?.Invoke(_handledPoints);
        }

        public bool TryToSpendPoints(int cost)
        {
            if (cost < 0)
                throw new Exception("cost cannot be less than 0");

            if (cost > _handledPoints)
                return false;

            _handledPoints -= cost;
            WalletBalanceChangedAction?.Invoke(_handledPoints);
            return true;
        }
    }
}