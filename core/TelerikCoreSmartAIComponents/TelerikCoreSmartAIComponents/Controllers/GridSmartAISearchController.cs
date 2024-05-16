using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using SmartComponents.LocalEmbeddings;
using TelerikCoreSmartAIComponents.Models;
using TelerikCoreSmartAIComponents.Services;

namespace TelerikCoreSmartAIComponents.Controllers
{
    public class GridSmartAISearchController : Controller
    {
        private CategoryService _categoryService;
        public GridSmartAISearchController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async  Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string filterQuery)
        {
            var data = await _categoryService.GetCategories();

            if (!string.IsNullOrEmpty(filterQuery))
            {
                var embeddings = GetEmbeddings(data);
                data = Search(data, embeddings, filterQuery);
            }

            return Json(data.ToDataSourceResult(request));
        }

        public List<Category> Search(IEnumerable<Category> data, Dictionary<int, EmbeddingF32> embeddings, string query)
        {
            using var embedder = new LocalEmbedder();
            var queryVector = embedder.Embed(query);

            return data.Where(p => LocalEmbedder.Similarity(embeddings[p.CategoryId], queryVector) > 0.65)
                        .ToList();
        }

        private Dictionary<int, EmbeddingF32> GetEmbeddings(IEnumerable<Category> data)
        {
            var result = new Dictionary<int, EmbeddingF32>();
            using var embedder = new LocalEmbedder();

            foreach (var category in data)
            {
                result.Add(category.CategoryId, embedder.Embed($"{category.Description} {category.CategoryName}"));
            }

            return result;
        }
    }
}
