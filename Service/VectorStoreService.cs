using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;

namespace Service;

#pragma warning disable SKEXP0001
public class VectorStoreService(
    VectorStoreCollection<string, VectorDataModel> vectorStoreCollection,
    ITextSearch<VectorDataModel> vectorStoreTextSearch)
{
    public async Task SaveToVectorStoreAsync(params List<VectorDataModel> dataModels)
    {
        await vectorStoreCollection.EnsureCollectionExistsAsync();
        await vectorStoreCollection.UpsertAsync(dataModels);
    }

    public async Task DeleteAllFromVectorStoreAsync()
    {
        await vectorStoreCollection.EnsureCollectionDeletedAsync();
        await vectorStoreCollection.EnsureCollectionExistsAsync();
    }

    public async Task<List<string>> SearchFromVectorStoreAsync(string query, int top = 10, int skip = 0)
    {
        var searchResult = new List<string>();
        await foreach (var result in (await vectorStoreTextSearch.SearchAsync(query,
                           new TextSearchOptions<VectorDataModel> { Top = top, Skip = skip })).Results)
        {
            searchResult.Add(result);
        }

        return searchResult;
    }
}

public class VectorDataModel
{
    [VectorStoreKey]
    [TextSearchResultName]
    public required string Id { get; set; }

    [VectorStoreData]
    [TextSearchResultValue]
    public required string Content { get; set; }

    [VectorStoreVector(3072)] public required string ContentEmbedding { get; set; }
}