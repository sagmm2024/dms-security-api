using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;

namespace SecurityApi.Grain.Test
{
    public sealed class ClusterFixture : IDisposable
    {
        public ClusterFixture()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurator>();
            Cluster = builder.Build();

            Cluster.Deploy();
        }

        public void Dispose()
        {
            Cluster.StopAllSilos();
        }

        public TestCluster Cluster { get; private set; }
    }

    public class TestSiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder hostBuilder)
        {
            hostBuilder.AddMemoryGrainStorageAsDefault()
                .ConfigureServices(services =>
            {
                // Configure inyección de bloom filter
                services.AddBloomFilter(setupAction =>
                {
                    setupAction.UseInMemory();
                });
            });
        }
    }
    [CollectionDefinition(ClusterCollection.Name)]
    public class ClusterCollection : ICollectionFixture<ClusterFixture>
    {
        public const string Name = "ClusterCollection";
    }
}
