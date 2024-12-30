using System.Net.Http.Json;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina;
using TickerAlert.Application.Interfaces.PriceMeasures.DolarArgentina.Dtos;

namespace TickerAlert.Infrastructure.ExternalServices.CotizacionDolares;

public sealed class DolarArgentinaService : IDolarArgentinaService
{
    private const string BaseApiUrl = "https://dolarapi.com/v1/";
    private readonly HttpClient _httpClient;

    public DolarArgentinaService(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient();

    public async Task<CotizacionDolar?> GetCotizacionDolarCCL()
    {
        string query = BaseApiUrl + "dolares/contadoconliqui";

        return await _httpClient.GetFromJsonAsync<CotizacionDolar?>(query);
    }
}
