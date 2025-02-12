using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Create;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;
using System.Net;

namespace Application.Test.PersonOperation.Commands.Create
{

    public class CreatePersonCommandTest
    {
        private readonly Mock<ILogger<CreatePersonCommandHandler>> _logger;
        private readonly Mock<IClusterClient> _cluster;
        private readonly CreatePersonCommandHandler _handler;
        private CancellationToken _cancellationToken;
        public CreatePersonCommandTest()
        {
            // Arrange
            _cluster = new Mock<IClusterClient>();
            _logger = new Mock<ILogger<CreatePersonCommandHandler>>();
            _cancellationToken = CancellationToken.None;

            _handler = new CreatePersonCommandHandler(_cluster.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_CreatePerson_Success()
        {
            // Arrange
            var request = new Fixture().Create<CreatePersonCommand>();
            var response = new Fixture().Create<PersonDto>();
            var personGrain = new Mock<IPersonGrains>();
            _cluster.Setup(x => x.GetGrain<IPersonGrains>(It.IsAny<Guid>(), It.IsAny<string>())).Returns(personGrain.Object);
            personGrain.Setup(x => x.CreateOrUpdate(It.IsAny<PersonInfo>())).ReturnsAsync(response);
            // Act
            var result = await _handler.Handle(request, _cancellationToken);

            // Assert
            result.Content.Message.Should().Be("Success");
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Content.PersonId.Should().Be(response.Id);
        }




    }
}
