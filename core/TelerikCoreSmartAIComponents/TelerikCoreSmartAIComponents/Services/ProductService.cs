using TelerikCoreSmartAIComponents.Models;

namespace TelerikCoreSmartAIComponents.Services
{
    public class ProductService
    {
        private HttpClient _http;

        private List<Product> _products;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return GetProductsInternal();
        }

        private async Task<IEnumerable<Product>> GetProductsInternal()
        {
            if (_products == null)
            {
                _products = (await _http.GetFromJsonAsync<IEnumerable<Product>>("https://demos.telerik.com/blazor-ui-service/api/Product/GetProducts")).ToList();
            }

            return _products;
        }
    }
}
