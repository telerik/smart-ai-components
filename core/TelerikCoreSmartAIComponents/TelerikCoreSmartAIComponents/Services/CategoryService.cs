using TelerikCoreSmartAIComponents.Models;

namespace TelerikCoreSmartAIComponents.Services
{
    public class CategoryService
    {
        private HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public Task<IEnumerable<Category>> GetCategories()
        {
            return _http.GetFromJsonAsync<IEnumerable<Category>>("https://demos.telerik.com/blazor-ui-service/api/Category/GetCategories");
        }
    }
}
