using ApiContracts;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infraestructure.ExternalApiService
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        public async Task<decimal> GetDollarExchangeRateAsync()
        {
            DateTime today = DateTime.Today;
            DateTime previousDay = today;

            // If today is a weekend, find the previous workday
            while (previousDay.DayOfWeek == DayOfWeek.Saturday || previousDay.DayOfWeek == DayOfWeek.Sunday)
            {
                previousDay = previousDay.AddDays(-1);
            }

            string apiUrl = $"https://mindicador.cl/api/dolar/{previousDay:dd-MM-yyyy}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var exchangeRateResponse = JsonConvert.DeserializeObject<DolarResponse>(responseBody);

                // Extract the latest exchange rate value
                var latestExchangeRate = exchangeRateResponse.Serie.FirstOrDefault()?.Valor;

                if (latestExchangeRate.HasValue)
                {
                    return Convert.ToDecimal(latestExchangeRate.Value);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exceptions that occurred during the HTTP request
                // Log or handle the exception as per your requirement
            }

            // Return a default value or throw an exception if necessary
            throw new Exception("Failed to retrieve the latest exchange rate.");
        }
    }
}