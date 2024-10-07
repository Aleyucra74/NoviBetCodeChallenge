using ECBProjectCodeNoviBet.Models;
using ECBProjectCodeNoviBet.Strategy.Interface;

namespace ECBProjectCodeNoviBet.Strategy
{
    public class SubtractFundsStrategy : IAdjustBalanceStrategy
    {
        public Task AdjustBalance(Wallet wallet, decimal amount)
        {
            if (wallet.Balance < amount)
            {
                throw new InvalidOperationException("Dinheiro insuficiente");
            }

            wallet.Balance -= amount;
            return Task.CompletedTask;
        }
    }
}
