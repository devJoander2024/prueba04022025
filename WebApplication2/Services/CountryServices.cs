using ServiceReference1;

namespace WebApplication2.Services
{
    public class CountryServices
    {

        private readonly CountryInfoServiceSoapTypeClient _client;

        public CountryServices()
        {
            _client = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        public async Task<ListOfCountryNamesByCodeResponse> GetCountriesAsync()
        {
            ListOfCountryNamesByCodeResponse resp = await _client.ListOfCountryNamesByCodeAsync();
            return resp;
        }
    }
}
