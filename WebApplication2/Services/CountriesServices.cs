using System.Text;
using System.Xml;
using WebApplication2.Dtos;

namespace WebApplication2.Services
{
    public class CountriesServices
    {
        private readonly HttpClient _httpClient;

        private string uri = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";

        public CountriesServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Countries>> GetAllCountries()
        {
            try
            {
                string soapRequest = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                  <soap12:Body>
                    <ListOfCountryNamesByName xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
                    </ListOfCountryNamesByName>
                  </soap12:Body>
                </soap12:Envelope>";

                StringContent content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error en la llamada al servicio SOAP: {response.StatusCode}");
                }

                string responseXml = await response.Content.ReadAsStringAsync();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);

                // Manejo de Namespaces en la respuesta SOAP
                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("m", "http://www.oorsprong.org/websamples.countryinfo");

                XmlNodeList countryNodes = doc.SelectNodes("//m:ListOfCountryNamesByNameResult/m:tCountryCodeAndName", nsManager);

                if (countryNodes == null || countryNodes.Count == 0)
                {
                    throw new Exception("No se encontraron países en la respuesta del servicio SOAP.");
                }

                List<Countries> countries = new List<Countries>();

                foreach (XmlNode countryNode in countryNodes)
                {
                    string isoCode = countryNode.SelectSingleNode("m:sISOCode", nsManager)?.InnerText ?? "";
                    string name = countryNode.SelectSingleNode("m:sName", nsManager)?.InnerText ?? "";

                    if (!string.IsNullOrEmpty(isoCode) && !string.IsNullOrEmpty(name))
                    {
                        countries.Add(new Countries { isoCode = isoCode, name = name });
                    }
                }

                return countries;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener la lista de países: {e.Message}");
                return new List<Countries>(); 
            }
        }

        public async Task<Countries> GetCountryByCode(string isoCode)
        {
            try
            {
                Countries c = await GetCountryMoney(isoCode);

               

                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <CapitalCity xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
      <sCountryISOCode>{isoCode}</sCountryISOCode>
    </CapitalCity>
  </soap:Body>
</soap:Envelope>";


                StringContent content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error en la llamada al servicio SOAP: {response.StatusCode}");
                }

                string responseXml = await response.Content.ReadAsStringAsync();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);

                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("m", "http://www.oorsprong.org/websamples.countryinfo");

                XmlNode countryNode = doc.SelectSingleNode("//m:CapitalCityResult", nsManager);

                if (countryNode == null )
                {
                    throw new Exception("No se encontraron países en la respuesta del servicio SOAP.");
                }

                Countries country = new Countries();
                country.codeMoneda = c.codeMoneda;
                country.moneda = c.moneda;
                country.name = countryNode.InnerText ?? "";
                //country.isoCode = countryNode.SelectSingleNode("m:sISOCode", nsManager)?.InnerText ?? "";

                return country;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener la lista de países: {e.Message}");
                return null;
            }
        }

        public async Task<Countries> GetCountryMoney(string isoCode)
        {
            try
            {
                Countries country = new Countries();

                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <CountryCurrency xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
      <sCountryISOCode>{isoCode}</sCountryISOCode>
    </CountryCurrency>
  </soap:Body>
</soap:Envelope>";

                StringContent content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error en la llamada al servicio SOAP: {response.StatusCode}");
                }

                string responseXml = await response.Content.ReadAsStringAsync();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);

                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("m", "http://www.oorsprong.org/websamples.countryinfo");

                XmlNode countryNode = doc.SelectSingleNode("//m:CountryCurrencyResult", nsManager);

                if (countryNode == null)
                {
                    throw new Exception("No se encontraron países en la respuesta del servicio SOAP.");
                }


                country.codeMoneda = countryNode.SelectSingleNode("m:sISOCode", nsManager)?.InnerText ?? "";
                country.moneda = countryNode.SelectSingleNode("m:sName", nsManager)?.InnerText ?? "";

                return country;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener la lista de países: {e.Message}");
                return null;
            }
        }
    }
}
