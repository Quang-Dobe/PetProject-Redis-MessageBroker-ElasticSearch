using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch;
using Services.Second.Services.Abstraction;
using Services.Second.Utils;

namespace Services.Second.Services
{
    public class ElasticServices : IElasticServices
    {
        private readonly ElasticHelper _helper;
        private readonly ILogger<ElasticServices> _logger;

        public ElasticServices(ElasticHelper helper, ILogger<ElasticServices> logger)
        {
            _helper = helper;
            _logger = logger;
        }

        public async Task<List<CreateIndexResponse>> CreateIndexAsync(ElasticsearchClient client, CancellationToken cancellationToken = default)
        {
            var responses = await _helper.CreateIndexAsync(client, cancellationToken);

            return responses;
        }

        public async Task<BulkResponse> BulkIndexAsync<T>(ElasticsearchClient client, IEnumerable<T> documents, CancellationToken cancellationToken = default)
        {
            var bulkResponse = await client.BulkAsync(b => b
                .Index(_helper.GetIndexName<T>())
                .IndexMany(documents), cancellationToken);

            return bulkResponse;
        }
    }

    public class ElasticClient : ElasticsearchClient
    {
        private readonly ElasticHelper _helper;

        public bool ExistsIndex { get; set; }

        public ElasticClient(ElasticHelper helper) : base(helper.CreateElasticSettings())
        {
            _helper = helper;
            ExistsIndex = false;
        }

        public async Task<List<CreateIndexResponse>> CreateIndexAsync(CancellationToken cancellationToken = default)
        {
            var response = await _helper.CreateIndexAsync(this);

            if (response.Count(x => x.IsSuccess()) == response.Count)
            {
                ExistsIndex = true;
            }

            return response;
        }
    }
}
