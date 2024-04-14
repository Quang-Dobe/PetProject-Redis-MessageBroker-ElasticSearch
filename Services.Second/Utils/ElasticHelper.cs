using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Transport;
using Services.Data.Core;

namespace Services.Second.Utils
{
    public class ElasticHelper
    {
        private readonly ElasticConfig _config;

        public ElasticHelper(ElasticConfig config)
        {
            _config = config;
        }

        public ElasticsearchClient CreateElasticClient()
        {
            return new ElasticsearchClient(CreateElasticSettings());
        }

        public async Task<List<CreateIndexResponse>> CreateIndexAsync(ElasticsearchClient client, CancellationToken cancellationToken = default)
        {
            var responses = new List<CreateIndexResponse>();

            // Create index for doca User
            var createUserIndexResponse = await client.Indices.CreateAsync(GetIndexName<User>(), descriptor => descriptor
                .Settings(settings => CreateDefaultIndexSettingsDescriptor(settings))
                .Mappings(mapping => CreateUserTypeMappingDescriptor(mapping)), cancellationToken);
            responses.Add(createUserIndexResponse);

            // Create index for doc Device
            var createDeviceIndexResponse = await client.Indices.CreateAsync(GetIndexName<Device>(), descriptor => descriptor
                .Settings(settings => CreateDefaultIndexSettingsDescriptor(settings))
                .Mappings(mapping => CreateDeviceTypeMappingDescriptor(mapping)), cancellationToken);
            responses.Add(createDeviceIndexResponse);

            return responses;
        }

        public ElasticsearchClientSettings CreateElasticSettings()
        {
            return new ElasticsearchClientSettings(new Uri(_config.Url))
                .CertificateFingerprint(_config.CertificateFingerprint)
                .Authentication(new BasicAuthentication(_config.UserName, _config.Password));
        }

        public string GetIndexName<T>()
        {
            return typeof(T).Name.ToLower();
        }

        #region Private methods

        private IndexSettingsDescriptor CreateDefaultIndexSettingsDescriptor(IndexSettingsDescriptor settings)
        {
            return settings
                .Analysis(analysis => analysis
                    .Normalizers(normalizers => normalizers
                        .Custom("sha_normalizer", normalizer => normalizer
                            .Filter(new[] { "lowercase" })
                        )
                    )
                    .Analyzers(analyzers => analyzers
                        .Custom("default", custom => custom
                            .Tokenizer("standard").Filter(new[]
                            {
                                "lowercase",
                                "stemmer"
                            })
                            )
                        .Custom("whitespace_reverse", custom => custom
                            .Tokenizer("whitespace").Filter(new[]
                            {
                                "lowercase",
                                "asciifolding",
                                "reverse"
                            })
                        )
                        .Custom("code_analyzer", custom => custom
                            .Tokenizer("whitespace").Filter(new[]
                            {
                                "word_delimiter_graph_filter",
                                "flatten_graph",
                                "lowercase",
                                "asciifolding",
                                "remove_duplicates"
                            })
                        )
                        .Custom("custom_path_tree", custom => custom
                            .Tokenizer("custom_hierarchy")
                        )
                        .Custom("custom_path_tree_reversed", custom => custom
                            .Tokenizer("custom_hierarchy_reversed")
                        )
                    )
                    .Tokenizers(tokenizers => tokenizers
                        .PathHierarchy("custom_hierarchy", tokenizer => tokenizer
                            .Delimiter("/"))
                        .PathHierarchy("custom_hierarchy_reversed", tokenizer => tokenizer
                            .Reverse(true).Delimiter("/"))
                    )
                    .TokenFilters(filters => filters
                        .WordDelimiterGraph("word_delimiter_graph_filter", filter => filter
                            .PreserveOriginal(true)
                        )
                    )
                );
        }

        private TypeMappingDescriptor CreateUserTypeMappingDescriptor(TypeMappingDescriptor mapping)
        {
            return mapping
            .Properties<User>(properties => properties
                .Keyword(properties => properties.Id, keyword => keyword
                    .IndexOptions(IndexOptions.Docs)
                    .Normalizer("sha_normalizer"))
                .Keyword(properties => properties.Name)
                .Keyword(properties => properties.Email)
                .Keyword(properties => properties.UserName)
            //.Text(properties => properties.Path, text => text
            //    .Fields(fields => fields
            //        .Text("tree", tree => tree.Analyzer("custom_path_tree"))
            //        .Text("tree_reversed", tree_reversed => tree_reversed.Analyzer("custom_path_tree_reversed"))
            //    ))
            //.Text(properties => properties.Filename, text => text
            //    .Analyzer("code_analyzer")
            //    .Store(true)
            //    .Fields(fields => fields
            //        .Text("reverse", tree => tree.Analyzer("whitespace_reverse"))
            //    ))
            //.Keyword(properties => properties.CommitHash, keyword => keyword
            //    .IndexOptions(IndexOptions.Docs)
            //    .Normalizer("sha_normalizer"))
            //.Text(properties => properties.Content, text => text
            //    .IndexOptions(IndexOptions.Positions)
            //    .Analyzer("code_analyzer")
            //    .TermVector(TermVectorOption.WithPositionsOffsetsPayloads)
            //    .Store(true))
            //.Keyword(properties => properties.Permalink)
            //.Date(properties => properties.LatestCommitDate)
            );
        }

        private TypeMappingDescriptor CreateDeviceTypeMappingDescriptor(TypeMappingDescriptor mapping)
        {
            return mapping
            .Properties<Device>(properties => properties
                .Keyword(properties => properties.Id, keyword => keyword
                    .IndexOptions(IndexOptions.Docs)
                    .Normalizer("sha_normalizer"))
                .Keyword(properties => properties.Name)
            );
        }

        #endregion
    }
}
