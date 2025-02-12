using AutoFixture;
using FluentAssertions;
using Orleans.TestingHost;
using Orleans.TestKit;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Grain.Test.Person;
[Collection(ClusterCollection.Name)]
public class PersonGrainsTests : TestKitBase
{
    private readonly Fixture _fixture = new();
    private readonly TestCluster _cluster;

    public PersonGrainsTests(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }
    [Fact]
    public async Task GetState_ReturnsExpectedState()
    {
        // Arrange
        var expectedState = _fixture.Create<PersonDto>();
        Guid _id = Guid.NewGuid();
        expectedState.Id = _id;
        var personGrain = _cluster.GrainFactory.GetGrain<IPersonGrains>(_id);
        await personGrain.CreateOrUpdate(new PersonInfo
        {
            Apellido = expectedState.Apellido,
            Nombre = expectedState.Nombre
        });

        // Act
        var result = await personGrain.GetState();

        // Assert
        result.Should().BeEquivalentTo(expectedState);
        result.Id.Should().Be(_id);
    }

    [Fact]
    public async Task CreateOrUpdate_UpdatesStateAndAddsToDirectory()
    {
        // Arrange
        Guid _id = Guid.NewGuid();
        var personGrain = _cluster.GrainFactory.GetGrain<IPersonGrains>(_id);
        var personInfo = _fixture.Create<PersonInfo>();

        // Act
        var result = await personGrain.CreateOrUpdate(personInfo);

        // Assert
        result.Nombre.Should().Be(personInfo.Nombre);
        result.Apellido.Should().Be(personInfo.Apellido);
    }

    [Fact]
    public async Task Delete_RemovesFromDirectoryAndClearsState()
    {
        // Arrange
        Guid _id = Guid.NewGuid();
        var personGrain = _cluster.GrainFactory.GetGrain<IPersonGrains>(_id);
        var expectedState = _fixture.Create<PersonDto>();
        expectedState.Id = _id;
        await personGrain.CreateOrUpdate(new PersonInfo
        {
            Apellido = expectedState.Apellido,
            Nombre = expectedState.Nombre
        });
        // Act
        await personGrain.Delete();

        // Assert
        var result = await personGrain.GetState();

        result.Nombre.Should().BeNullOrEmpty();
        result.Apellido.Should().BeNullOrEmpty();
    }
}
