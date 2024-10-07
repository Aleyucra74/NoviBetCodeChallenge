using ECBProjectCodeNoviBet.Models;

namespace ECBProjectCodeNoviBet.Strategy.Interface
{
    public interface IAdjustBalanceStrategy
    {
        Task AdjustBalance(Wallet wallet, decimal amount);
    }
}
