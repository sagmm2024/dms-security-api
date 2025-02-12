using Andreani.Arq.Orleans.Abstractions.Directories;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Update;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;
using System.Net;

namespace Application.Test.PersonOperation.Commands.Update
{
    public class UpdatePersonCommandTest
    {
        private readonly Mock<IClusterClient> _cluster;
        private readonly Mock<ILogger<UpdatePersonHandler>> _logger;

        private readonly UpdatePersonHandler _handler;
        private CancellationToken _cancellationToken;

        public UpdatePersonCommandTest()
        {
            // Arrange
            _cluster = new Mock<IClusterClient>();
            _logger = new Mock<ILogger<UpdatePersonHandler>>();
            _cancellationToken = CancellationToken.None;

            _handler = new UpdatePersonHandler(_cluster.Object);
        }
        [Fact]
        public async Task Handler_UpdatePerson_Success()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            request.PersonId = Guid.NewGuid().ToString();
            var response = new Fixture().Create<PersonDto>();
            var personGrain = new Mock<IPersonGrains>();
            var directoryGrain = new Mock<IBigDirectoryGuidGrain>();
            _cluster.Setup(x => x.GetGrain<IBigDirectoryGuidGrain>(It.IsAny<string>(), It.IsAny<string>())).Returns(directoryGrain.Object);
            directoryGrain.Setup(x => x.Exist(It.IsAny<Guid>())).ReturnsAsync(true);
            _cluster.Setup(x => x.GetGrain<IPersonGrains>(It.IsAny<Guid>(), It.IsAny<string>())).Returns(personGrain.Object);
            personGrain.Setup(x => x.CreateOrUpdate(It.IsAny<PersonInfo>())).ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Should().NotBeNull();
            result.Content.Id.Should().Be(response.Id);
        }

        [Fact]
        public async Task Handler_UpdatePerson_PersonNotExist()
        {
            // Arrange
            var request = new Fixture().Create<UpdatePersonCommand>();
            request.PersonId = Guid.NewGuid().ToString();
            var directoryGrain = new Mock<IBigDirectoryGuidGrain>();
            _cluster.Setup(x => x.GetGrain<IBigDirectoryGuidGrain>(It.IsAny<string>(), It.IsAny<string>())).Returns(directoryGrain.Object);
            directoryGrain.Setup(x => x.Exist(It.IsAny<Guid>())).ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.IsValid.Should().BeFalse();
        }

    }
}
