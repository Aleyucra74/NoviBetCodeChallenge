using ECBGatewayLibrary;
using ECBProjectCodeNoviBet.Data;
using ECBProjectCodeNoviBet.Models;
using ECBProjectCodeNoviBet.Repository.Interface;
using ECBProjectCodeNoviBet.Strategy;
using ECBProjectCodeNoviBet.Strategy.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ECBProjectCodeNoviBet.Controllers
{
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IWalletRepository _walletRepository;
        private readonly ECBClientHttp _eCBClientHttp;
        private readonly ECBParserXML _parserXML;
        private readonly BaseStrategy _baseStrategy;

        public WalletController(DatabaseContext context,
            IWalletRepository walletRepository,
            ECBClientHttp eCBClientHttp,
            ECBParserXML eCBParserXML,
            BaseStrategy baseStrategy
            )
        {
            _context = context;
            _walletRepository = walletRepository;
            _eCBClientHttp = eCBClientHttp;
            _parserXML = eCBParserXML;
            _baseStrategy = baseStrategy;
        }

        [HttpPost]
        public async Task<ActionResult> CreateWallet(Wallet walletBody)
        {
            var wallet = new Wallet {
                Id = walletBody.Id,
                Balance = walletBody.Balance,
                Currency = walletBody.Currency,
            };

            await _walletRepository.CreateWallet(wallet);

            return Ok();
        }

        [HttpPost]
        [Route("{walletId}")]
        public async Task<ActionResult> RetrieveWalletBalance(long walletId, string currency)
        {
            var wallet = await _walletRepository.GetById(walletId);

            if (wallet == null)
            {
                return NotFound();
            }

            if (currency != null && currency != wallet.Currency)
            {
                var rates = await _eCBClientHttp.GetRateData();
                var ratesParser = _parserXML.ParserXMl(rates);

                var toRate = ratesParser.FirstOrDefault(x => x.Currency == currency);
                var convertBalance = wallet.Balance * toRate.Rate;

                return Ok(new { Balance = convertBalance, Currency = currency });
            }
            //same currency
            return Ok(new { Balance = wallet.Balance, Currency = currency });
        }

        [HttpPost]
        [Route("{walletId}/adjustbalance")]
        public async Task<ActionResult> AdjustWalletBalance(long walletId, decimal amount, string currency, string strategy)
        {
            var wallet = await _walletRepository.GetById(walletId);
            if (wallet == null)
            {
                return NotFound();
            }

            if (currency != null && currency != wallet.Currency)
            {
                var rates = await _eCBClientHttp.GetRateData();
                var ratesParser = _parserXML.ParserXMl(rates);

                var toRate = ratesParser.FirstOrDefault(x => x.Currency == currency);
                amount = amount * toRate.Rate;
            }

            switch (strategy)
            {
                case("AddFundStrategy"):
                    _baseStrategy.DefineStrategy(new AddFundsStrategy());
                    _baseStrategy.FundsStrategy(wallet, amount);
                    break;
                case("ForceSubtractFundsStrategy"):
                    _baseStrategy.DefineStrategy(new ForceSubtractFundsStrategy());
                    _baseStrategy.FundsStrategy(wallet, amount);
                    break;
                case ("SubtractFundsStrategy"):
                    _baseStrategy.DefineStrategy(new SubtractFundsStrategy());
                    _baseStrategy.FundsStrategy(wallet, amount);
                    break;
            }

            await _walletRepository.UpdateWallet(wallet);
            return Ok(new { Balance = wallet.Balance });
        }


    }
}
