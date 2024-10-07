using ECBProjectCodeNoviBet.Models;

namespace ECBProjectCodeNoviBet.Repository.Interface
{
    public interface IWalletRepository
    {

        Task<Wallet> GetById(long walletId);
        Task CreateWallet(Wallet wallet);
        Task UpdateWallet(Wallet wallet);

    }
}
