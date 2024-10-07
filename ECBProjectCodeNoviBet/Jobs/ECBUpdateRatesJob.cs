using ECBProjectCodeNoviBet.Data;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ECBGatewayLibrary
{
    public class ECBUpdateRatesJob : IJob
    {
        private readonly ECBClientHttp _ecbClientHttp;
        private readonly ECBParserXML _ecbParserXML;
        private readonly IServiceProvider _serviceProvider;

        public ECBUpdateRatesJob(ECBClientHttp ecbClientHttp,
                                 ECBParserXML ecbParserXML, 
                                 IServiceProvider serviceProvider
            )
        {
            _ecbClientHttp = ecbClientHttp;
            _ecbParserXML = ecbParserXML;
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var xmlData = await _ecbClientHttp.GetRateData();
            var rates = _ecbParserXML.ParserXMl(xmlData);

            using (var db = _serviceProvider.CreateAsyncScope())
            {
                var dbContext = db.ServiceProvider.GetRequiredService<DatabaseContext>();
                await RatesUpdateDB(dbContext, rates);
            }
        }

        public async Task RatesUpdateDB(DatabaseContext dbContext, List<ECBRateModel> rates)
        {
            var sql = @"
                MERGE
                    dbo.Rates as Origem
                USING 
                    (VALUES {0})  as Destino (currency, rate, date) ON Origem.Currency = Destino.Currency AND Origem.date = Destino.date
                WHEN MATCHED THEN
                    UPDATE SET Origem.rate = Destino.rate
                WHEN NOT MATCHED THEN
                    INSERT INTO (currency, rate, date) 
                        VALUES (Destino.currency, Destino.rate, Destino.date);";

            var values = string.Join(", ", rates.Select(r => $"('{r.Currency}', {r.Rate}, '{DateTime.UtcNow:dd-MM-yyyy}')"));
            var mergeCommand = string.Format(sql, values);

            await dbContext.Database.CommitTransactionAsync();
        }
    }
}
