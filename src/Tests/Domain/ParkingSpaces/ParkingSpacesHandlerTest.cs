using AutoMapper;
using FluentAssertions;
using Moq;
using Parking.Control.Domain.Commands.ParkingSpaces.CreateParkingSpace;
using Parking.Control.Domain.Commands.ParkingSpaces.RemoveParkingSpace;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Enums;
using Parking.Control.Domain.Handlers;
using Parking.Control.Domain.Interfaces.Repositories;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailabeSpaces;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType;
using Parking.Control.Domain.Queries.ParkingSpace.GetQuantitySpaces;

namespace Parking.Control.Tests.Domain.ParkingSpaces
{
    public class ParkingSpacesHandlerTest : BaseTest
    {
        private readonly Mock<IVehicleRepository> _vehicleRepository;
        private readonly Mock<IParkingSpaceRepository> _parkingSpaceRepository;
        private readonly Mock<IMapper> _mapper;

        public ParkingSpacesHandlerTest()
        {
            _vehicleRepository = new();
            _parkingSpaceRepository = new();
            _mapper = new();
        }

        [Fact]
        public async Task HandleCreate_ShouldReturnSuccess_WhenCommandIsValid()
        {
            #region Arrange
            var command = _fixture.Create<CreateParkingSpaceCommand>();

            _fixture.Customize<CreateParkingSpaceCommandResponse>(resp => resp
                .With(resp => resp.Available, command.Available)
                .With(resp => resp.Type, command.Type));
            var response = _fixture.Create<CreateParkingSpaceCommandResponse>();

            var parkingSpace = _fixture.Create<ParkingSpace>();

            _mapper.Setup(map => map.Map<ParkingSpace>(It.IsAny<CreateParkingSpaceCommand>())).Returns(parkingSpace);
            _mapper.Setup(map => map.Map<CreateParkingSpaceCommandResponse>(It.IsAny<ParkingSpace>())).Returns(response);

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<CreateParkingSpaceCommandResponse>(result);
            result.Available.Should().Be(response.Available);
            result.Type.Should().Be(response.Type);
            _parkingSpaceRepository.Verify(repository => repository.CreateAsync(It.IsAny<ParkingSpace>()), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleRemove_ShouldThrowException_WhenSpaceDoesNotExists()
        {
            #region Arrange
            var command = _fixture.Create<RemoveParkingSpaceCommand>();

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var ex = await Record.ExceptionAsync(async () => await handle.Handle(command, CancellationToken.None));
            #endregion

            #region Assert
            ex.Should().NotBeNull();
            Assert.IsType<Exception>(ex);
            Assert.Contains("Vaga não encontrada", ex.Message);
            _parkingSpaceRepository.Verify(repository => repository.RemoveAsync(It.IsAny<int>()), Times.Once);
            _vehicleRepository.Verify(repository => repository.RemoveParkedVehicleAsync(It.IsAny<Vehicle>()), Times.Never);
            #endregion
        }

        [Fact]
        public async Task HandleRemove_ShouldReturnSuccess_WhenCommandIsValid()
        {
            #region Arrange
            var command = _fixture.Create<RemoveParkingSpaceCommand>();
            var parkingSpace = _fixture.Create<ParkingSpace>();

            _parkingSpaceRepository.Setup(repository => repository.RemoveAsync(It.IsAny<int>())).ReturnsAsync(parkingSpace);

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<RemoveParkingSpaceCommandResponse>(result);
            result.IsRemoved.Should().BeTrue();
            _parkingSpaceRepository.Verify(repository => repository.RemoveAsync(It.IsAny<int>()), Times.Once);
            _vehicleRepository.Verify(repository => repository.RemoveParkedVehicleAsync(It.IsAny<Vehicle>()), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleGetAvailableSpaces_ShouldReturnSuccess()
        {
            #region Arrange
            var command = _fixture.Create<GetAvailableSpacesQuery>();
            var avaialableSpaces = _fixture.CreateMany<ParkingSpace>(2).ToList();

            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(avaialableSpaces);

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<GetAvailableSpacesQueryResponse>(result);
            result.Count.Should().Be(2);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleGetQuantitySpaces_ShouldReturnSuccess()
        {
            #region Arrange
            var command = _fixture.Create<GetQuantitySpacesQuery>();

            _parkingSpaceRepository.Setup(repository => repository.GetQuantityAsync()).ReturnsAsync(2);

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<GetQuantitySpacesQueryResponse>(result);
            result.Count.Should().Be(2);
            _parkingSpaceRepository.Verify(repository => repository.GetQuantityAsync(), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleGetAvailableSpacesByType_ShouldReturnSuccess()
        {
            #region Arrange
            _fixture.Customize<GetAvailableSpacesByTypeQuery>(command => command.With(command => command.Type, SpaceType.Motorbike));
            var command = _fixture.Create<GetAvailableSpacesByTypeQuery>();

            _fixture.Customize<ParkingSpace>(space => space.With(space => space.Type, (int)command.Type));
            var avaialableSpaces = _fixture.CreateMany<ParkingSpace>(2).ToList();

            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(avaialableSpaces);

            var handle = new ParkingSpacesHandler(
                _parkingSpaceRepository.Object,
                _vehicleRepository.Object,
                _mapper.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<GetAvailableSpacesByTypeQueryResponse>(result);
            result.Count.Should().Be(2);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            #endregion
        }
    }
}
