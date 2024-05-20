using Microsoft.AspNetCore.Mvc;
using SmartComponents.LocalEmbeddings;
using TelerikCoreSmartAIComponents.Models;
using TelerikCoreSmartAIComponents.Services;

namespace TelerikCoreSmartAIComponents.Controllers
{
    public class ComboBoxSmartSearchController : Controller
    {
        private ProductService _productService;

        public ComboBoxSmartSearchController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read(string text)
        {
            var data = await _productService.GetProducts();

            if (!string.IsNullOrEmpty(text))
            {
                var embeddings = GetEmbeddings(data);
                data = Search(data, embeddings, text);
            }

            return Json(data.Select(d => new Product { ProductName = d.ProductName, ProductId = d.ProductId }));
        }

        public List<Product> Search(IEnumerable<Product> data, Dictionary<int, EmbeddingF32> embeddings, string query)
        {
            using var embedder = new LocalEmbedder();
            var queryVector = embedder.Embed(query);

            return data.Where(p => LocalEmbedder.Similarity(embeddings[p.ProductId], queryVector) > 0.65)
                        .ToList();
        }

        private Dictionary<int, EmbeddingF32> GetEmbeddings(IEnumerable<Product> data)
        {
            var result = new Dictionary<int, EmbeddingF32>();
            using var embedder = new LocalEmbedder();

            foreach (var product in data)
            {
                result.Add(product.ProductId, embedder.Embed($"{product.ProductName} {product.CategoryName}"));
            }

            return result;
        }
    }
}
