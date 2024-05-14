using System.Net.Http.Json;
using TelerikBlazorSmartAIComponents.Client.Models;

namespace TelerikBlazorSmartAIComponents.Client.Services
{
    public class ProductService
    {
        private HttpClient _http;

        private List<ProductDto> _products;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public Task<IEnumerable<ProductDto>> GetProducts()
        {
            return GetProductsInternal();
        }

        private async Task<IEnumerable<ProductDto>> GetProductsInternal()
        {
            if (_products == null)
            {
                _products = (await _http.GetFromJsonAsync<IEnumerable<ProductDto>>("https://demos.telerik.com/blazor-ui-service/api/Product/GetProducts")).ToList();
            }

            return _products;
        }
    }
}
