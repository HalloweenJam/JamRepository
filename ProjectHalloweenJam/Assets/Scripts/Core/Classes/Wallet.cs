using System;

namespace Core.Classes
{
    public class Wallet
    {
        public Action<int> WalletBalanceChangedAction;
        public int HandledPoints { get; private set; }

        public void DepositPoints(int points)
        {
            if (points <= 0)
                throw new Exception("money amount cannot be less than 0");

            HandledPoints += points;
            WalletBalanceChangedAction?.Invoke(HandledPoints);
        }

        public bool TryToSpendPoints(int cost)
        {
            if (cost < 0)
                throw new Exception("cost cannot be less than 0");

            if (cost > HandledPoints)
                return false;

            HandledPoints -= cost;
            WalletBalanceChangedAction?.Invoke(HandledPoints);
            return true;
        }
    }
}