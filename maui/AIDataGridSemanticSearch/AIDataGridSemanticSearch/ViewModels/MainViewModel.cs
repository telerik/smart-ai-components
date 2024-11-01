using SmartComponents.LocalEmbeddings;
using Telerik.Maui.Controls.DataGrid;

namespace AIDataGridSemanticSearch;

public class MainViewModel
{
    private readonly LocalEmbedder embedder = new LocalEmbedder();
    
    public MainViewModel()
    {
        this.Data = ProductService.CreateData();
        this.SemanticSearch = this.ExecuteSemanticSearch;
    }

    public IList<Product> Data { get; private set; }

    public Action<DataGridSearchProbe> SemanticSearch { get; }

    private void ExecuteSemanticSearch(DataGridSearchProbe probe)
    {
        EmbeddingF32 searchTextEmbedding = this.embedder.Embed(probe.SearchTerms[0]);
        EmbeddingF32 itemTextEmbedding = this.embedder.Embed(probe.ItemText);

        if (LocalEmbedder.Similarity(searchTextEmbedding, itemTextEmbedding) > 0.65)
        {
            probe.Matches.Add(new DataGridSearchMatch());
        }
    }
}
