using ServiceReference1;
using ServiceReference2;

namespace WebApplication2.Services
{
    public class CalculadoraService
    {
        private readonly CalculatorSoapClient _client;

        public CalculadoraService()
        {
            _client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
        }

        public async Task<int> sumar(int num1, int num2)
        {
            try
            {
                return await _client.AddAsync(num1, num2);

            }catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }


        }
    }
}
