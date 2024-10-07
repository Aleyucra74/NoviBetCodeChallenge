using ECBProjectCodeNoviBet.Data;
using ECBProjectCodeNoviBet.Models;
using ECBProjectCodeNoviBet.Repository.Interface;

namespace ECBProjectCodeNoviBet.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly DatabaseContext _context;

        public WalletRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetById(long walletId)
        {
            return await _context.Wallets.FindAsync(walletId);
        }

        public async Task CreateWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
