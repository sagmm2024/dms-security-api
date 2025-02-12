using AutoFixture;
using FluentAssertions;
using Orleans.TestingHost;
using Orleans.TestKit;
using SecurityApi.Grain.Interface.Exceptions;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Grain.Test.User
{
    [Collection(ClusterCollection.Name)]
    public class UserGrainsTests : TestKitBase
    {
        private readonly Fixture _fixture = new();
        private readonly TestCluster _cluster;

        public UserGrainsTests(ClusterFixture fixture)
        {
            _cluster = fixture.Cluster;
        }

        [Fact]
        public async Task Register_CreatesNewUser()
        {
            // Arrange
            string _id = "Register_CreatesNewUser";
            string password = "testPassword";
            var userInfo = _fixture.Create<PersonInfo>();
            var userGrain = _cluster.GrainFactory.GetGrain<IUserGrains>(_id);

            // Act
            await userGrain.Register(password, userInfo);

            // Assert
            var result = await userGrain.GetState();
            result.Username.Should().Be(_id);
            result.Info.Should().BeEquivalentTo(userInfo);
        }

        [Fact]
        public async Task Login_ValidatesUserPassword()
        {
            // Arrange
            string _id = "Login_ValidatesUserPassword";
            string password = "testPassword";
            var userInfo = _fixture.Create<PersonInfo>();
            var userGrain = _cluster.GrainFactory.GetGrain<IUserGrains>(_id);
            await userGrain.Register(password, userInfo);

            // Act
            var result = await userGrain.Login(password);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_ThrowsInvalidLoginExceptionForInvalidPassword()
        {
            // Arrange
            string _id = "Login_ThrowsInvalidLoginExceptionForInvalidPassword";
            string password = "testPassword";
            string wrongPassword = "wrongPassword";
            var userInfo = _fixture.Create<PersonInfo>();
            var userGrain = _cluster.GrainFactory.GetGrain<IUserGrains>(_id);
            await userGrain.Register(password, userInfo);

            // Act
            Func<Task> action = async () => await userGrain.Login(wrongPassword);

            // Assert
            await action.Should().ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task GetState_ReturnsUserState()
        {
            // Arrange
            string _id = "GetState_ReturnsUserState";
            string password = "testPassword";
            var userInfo = _fixture.Create<PersonInfo>();
            var userGrain = _cluster.GrainFactory.GetGrain<IUserGrains>(_id);
            await userGrain.Register(password, userInfo);

            // Act
            var result = await userGrain.GetState();

            // Assert
            result.Username.Should().Be(_id);
            result.Info.Should().BeEquivalentTo(userInfo);
        }
    }

}
