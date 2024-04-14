using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch;

namespace Services.Second.Services.Abstraction
{
    public interface IElasticServices
    {
        Task<List<CreateIndexResponse>> CreateIndexAsync(ElasticsearchClient client, CancellationToken cancellationToken = default);

        Task<BulkResponse> BulkIndexAsync<T>(ElasticsearchClient client, IEnumerable<T> documents, CancellationToken cancellationToken = default);
    }
}
