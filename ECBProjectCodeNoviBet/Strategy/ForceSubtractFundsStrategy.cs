using ECBProjectCodeNoviBet.Models;
using ECBProjectCodeNoviBet.Strategy.Interface;

namespace ECBProjectCodeNoviBet.Strategy
{
    public class ForceSubtractFundsStrategy : IAdjustBalanceStrategy
    {
        public Task AdjustBalance(Wallet wallet, decimal amount)
        {
            wallet.Balance -= amount;
            return Task.CompletedTask;
        }
    }
}
