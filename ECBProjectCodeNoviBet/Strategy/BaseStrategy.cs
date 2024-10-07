using ECBProjectCodeNoviBet.Models;
using ECBProjectCodeNoviBet.Strategy.Interface;

namespace ECBProjectCodeNoviBet.Strategy
{
    public class BaseStrategy
    {
        private IAdjustBalanceStrategy AdjustBalance;

        public BaseStrategy(IAdjustBalanceStrategy adjustBalance)
        {
            AdjustBalance = adjustBalance;
        }

        public void DefineStrategy(IAdjustBalanceStrategy adjustBalanceStrategy)
        {
            AdjustBalance = adjustBalanceStrategy;
        }

        public void FundsStrategy(Wallet wallet, decimal amount)
        {
            AdjustBalance.AdjustBalance(wallet, amount);
        }
    }
}
