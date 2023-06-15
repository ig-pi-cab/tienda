using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using TiendaFront.Models;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProducts()
    {
        var response = await _httpClient.GetAsync("https://localhost:5001/api/productos");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<Product>>(json);

        return products;
    }
    public async Task<Product> GetProductById(int productId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:5001/api/productos/{productId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<Product>(json);

        return product;
    }
}
