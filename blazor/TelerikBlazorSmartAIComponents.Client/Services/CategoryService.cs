using System.Net.Http.Json;
using TelerikBlazorSmartAIComponents.Client.Models;

namespace TelerikBlazorSmartAIComponents.Client.Services
{
    public class CategoryService
    {
        private HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public Task<IEnumerable<CategoryDto>> GetCategories()
        {
            return _http.GetFromJsonAsync<IEnumerable<CategoryDto>>("https://demos.telerik.com/blazor-ui-service/api/Category/GetCategories");
        }
    }
}
