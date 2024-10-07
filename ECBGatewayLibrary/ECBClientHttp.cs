namespace ECBGatewayLibrary
{
    public class ECBClientHttp
    {
        private readonly HttpClient _httpClient;

        //public ECBClientHttp() {}

        public ECBClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //implementing http client request
        public async Task<string> GetRateData()
        {
            var response = await _httpClient.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}