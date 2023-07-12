using AutoMapper;
using FluentAssertions;
using Moq;
using Parking.Control.Domain.Commands.Vehicles.ParkVehicle;
using Parking.Control.Domain.Commands.Vehicles.RemoveVehicle;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Enums;
using Parking.Control.Domain.Handlers;
using Parking.Control.Domain.Interfaces.Repositories;
using Shouldly;

namespace Parking.Control.Tests.Domain.Vehicles
{
    public class VehicleHandlerTest : BaseTest
    {
        private readonly Mock<IVehicleRepository> _vehicleRepository;
        private readonly Mock<IParkingSpaceRepository> _parkingSpaceRepository;
        private readonly Mock<IMapper> _mapper;

        public VehicleHandlerTest()
        {
            _vehicleRepository = new();
            _parkingSpaceRepository = new();
            _mapper = new();
        }

        [Fact]
        public async Task HandleParkVehicle_ShouldThrowException_WhenVehicleDoesExists()
        {
            #region Arrange
            var command = _fixture.Create<ParkVehicleCommand>();

            _vehicleRepository.Setup(repository => repository.CheckParkedCarAsync(It.IsAny<string>())).ReturnsAsync(true);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var ex = await Record.ExceptionAsync(async () => await handle.Handle(command, CancellationToken.None));
            #endregion

            #region Assert
            ex.Should().NotBeNull();
            Assert.IsType<Exception>(ex);
            Assert.Contains("Veiculo já estacionado", ex.Message);
            _vehicleRepository.Verify(repository => repository.CheckParkedCarAsync(It.IsAny<string>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Never);
            _vehicleRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>()), Times.Never);
            _parkingSpaceRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<List<ParkingSpace>>(), It.IsAny<Vehicle>()), Times.Never);
            #endregion
        }

        [Fact]
        public async Task HandleParkVehicle_ShouldThrowException_WhenUnavailableSpaces()
        {
            #region Arrange
            var command = _fixture.Create<ParkVehicleCommand>();
            var availableSpaces = _fixture.CreateMany<ParkingSpace>(0).ToList();

            _vehicleRepository.Setup(repository => repository.CheckParkedCarAsync(It.IsAny<string>())).ReturnsAsync(false);
            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(availableSpaces);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var ex = await Record.ExceptionAsync(async () => await handle.Handle(command, CancellationToken.None));
            #endregion

            #region Assert
            ex.Should().NotBeNull();
            Assert.IsType<Exception>(ex);
            Assert.Contains("Nenhuma vaga disponível!", ex.Message);
            _vehicleRepository.Verify(repository => repository.CheckParkedCarAsync(It.IsAny<string>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            _vehicleRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>()), Times.Never);
            _parkingSpaceRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<List<ParkingSpace>>(), It.IsAny<Vehicle>()), Times.Never);
            #endregion
        }

        [Fact]
        public async Task HandleParkVehicle_ShouldThrowException_WhenUnavailableSpacesByType()
        {
            #region Arrange
            _fixture.Customize<ParkVehicleCommand>(command => command.With(command => command.Type, VehicleType.Van));
            var command = _fixture.Create<ParkVehicleCommand>();

            _fixture.Customize<ParkingSpace>(command => command.With(command => command.Type, (int)SpaceType.Motorbike));
            var availableSpaces = _fixture.CreateMany<ParkingSpace>(1).ToList();

            _vehicleRepository.Setup(repository => repository.CheckParkedCarAsync(It.IsAny<string>())).ReturnsAsync(false);
            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(availableSpaces);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var ex = await Record.ExceptionAsync(async () => await handle.Handle(command, CancellationToken.None));
            #endregion

            #region Assert
            ex.Should().NotBeNull();
            Assert.IsType<Exception>(ex);
            Assert.Contains("Nenhuma vaga disponível pro tipo de veiculo especificado!", ex.Message);
            _vehicleRepository.Verify(repository => repository.CheckParkedCarAsync(It.IsAny<string>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            _vehicleRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>()), Times.Never);
            _parkingSpaceRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<List<ParkingSpace>>(), It.IsAny<Vehicle>()), Times.Never);
            #endregion
        }

        [Fact]
        public async Task HandleParkVehicle_ShouldReturnSuccess_WhenVehicleTypeIsMotorbike()
        {
            #region Arrange
            _fixture.Customize<ParkVehicleCommand>(command => command.With(command => command.Type, VehicleType.Motorbike));
            var command = _fixture.Create<ParkVehicleCommand>();

            _fixture.Customize<ParkingSpace>(command => command.With(command => command.Type, (int)SpaceType.Motorbike));
            var availableSpaces = _fixture.CreateMany<ParkingSpace>(2).ToList();

            _fixture.Customize<Vehicle>(vehicle => vehicle
                .With(vehicle => vehicle.Type, (int)command.Type)
                .With(vehicle => vehicle.LicensePlate, command.LicensePlate)
                .Without(vehicle => vehicle.ParkingSpaces));
            var vehicle = _fixture.Create<Vehicle>();

            _fixture.Customize<ParkVehicleCommandResponse>(resp => resp
                .With(resp => resp.LicensePlate, vehicle.LicensePlate)
                .With(resp => resp.Type, (VehicleType)vehicle.Type));
            var response = _fixture.Create<ParkVehicleCommandResponse>();

            _mapper.Setup(map => map.Map<Vehicle>(It.IsAny<ParkVehicleCommand>())).Returns(vehicle);
            _mapper.Setup(map => map.Map<ParkVehicleCommandResponse>(It.IsAny<Vehicle>())).Returns(response);
            _vehicleRepository.Setup(repository => repository.CheckParkedCarAsync(It.IsAny<string>())).ReturnsAsync(false);
            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(availableSpaces);
            _vehicleRepository.Setup(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>())).ReturnsAsync(vehicle);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<ParkVehicleCommandResponse>(result);
            result.Type.Should().Be((VehicleType)vehicle.Type);
            _vehicleRepository.Verify(repository => repository.CheckParkedCarAsync(It.IsAny<string>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            _vehicleRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<List<ParkingSpace>>(), It.IsAny<Vehicle>()), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleParkVehicle_ShouldReturnSuccess_WhenVehicleTypeIsVan()
        {
            #region Arrange
            _fixture.Customize<ParkVehicleCommand>(command => command.With(command => command.Type, VehicleType.Van));
            var command = _fixture.Create<ParkVehicleCommand>();

            _fixture.Customize<ParkingSpace>(command => command.With(command => command.Type, (int)SpaceType.Car));
            var availableSpaces = _fixture.CreateMany<ParkingSpace>(3).ToList();

            _fixture.Customize<Vehicle>(vehicle => vehicle
                .With(vehicle => vehicle.Type, (int)command.Type)
                .With(vehicle => vehicle.LicensePlate, command.LicensePlate)
                .Without(vehicle => vehicle.ParkingSpaces));
            var vehicle = _fixture.Create<Vehicle>();

            _fixture.Customize<ParkVehicleCommandResponse>(resp => resp
                .With(resp => resp.LicensePlate, vehicle.LicensePlate)
                .With(resp => resp.Type, (VehicleType)vehicle.Type));
            var response = _fixture.Create<ParkVehicleCommandResponse>();

            _mapper.Setup(map => map.Map<Vehicle>(It.IsAny<ParkVehicleCommand>())).Returns(vehicle);
            _mapper.Setup(map => map.Map<ParkVehicleCommandResponse>(It.IsAny<Vehicle>())).Returns(response);
            _vehicleRepository.Setup(repository => repository.CheckParkedCarAsync(It.IsAny<string>())).ReturnsAsync(false);
            _parkingSpaceRepository.Setup(repository => repository.GetAvailableSpacesAsync()).ReturnsAsync(availableSpaces);
            _vehicleRepository.Setup(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>())).ReturnsAsync(vehicle);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            Assert.IsType<ParkVehicleCommandResponse>(result);
            result.Type.Should().Be((VehicleType)vehicle.Type);
            _vehicleRepository.Verify(repository => repository.CheckParkedCarAsync(It.IsAny<string>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.GetAvailableSpacesAsync(), Times.Once);
            _vehicleRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<Vehicle>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.ParkVehicleAsync(It.IsAny<List<ParkingSpace>>(), It.IsAny<Vehicle>()), Times.Once);
            #endregion
        }

        [Fact]
        public async Task HandleRemoveVehicle_ShouldThrowException_WhenVehicleDoesNotExists()
        {
            #region Arrange
            var command = _fixture.Create<RemoveParkedVehicleCommand>();

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var ex = await Record.ExceptionAsync(async () => await handle.Handle(command, CancellationToken.None));
            #endregion

            #region Assert
            ex.Should().NotBeNull();
            Assert.IsType<Exception>(ex);
            Assert.Contains("Veiculo não encontrado", ex.Message);
            _vehicleRepository.Verify(repository => repository.GetVehicleAsync(It.IsAny<string>()), Times.Once);
            _vehicleRepository.Verify(repository => repository.RemoveParkedVehicleAsync(It.IsAny<Vehicle>()), Times.Never);
            _parkingSpaceRepository.Verify(repository => repository.RemoveParkedVehiclesAsync(It.IsAny<List<ParkingSpace>>()), Times.Never);
            #endregion
        }

        [Fact]
        public async Task HandleRemoveVehicle_ShouldReturnSuccess_WhenVehicleDoesExists()
        {
            #region Arrange
            var command = _fixture.Create<RemoveParkedVehicleCommand>();
            var vehicle = _fixture.Create<Vehicle>();

            _vehicleRepository.Setup(repository => repository.GetVehicleAsync(It.IsAny<string>())).ReturnsAsync(vehicle);

            var handle = new VehiclesHandler(
                _vehicleRepository.Object,
                _mapper.Object,
                _parkingSpaceRepository.Object);
            #endregion

            #region Act
            var result = await handle.Handle(command, CancellationToken.None);
            #endregion

            #region Assert
            result.Should().NotBeNull();
            result.IsRemoved.ShouldBe(true);
            Assert.IsType<RemoveParkedVehicleCommandResponse>(result);
            _vehicleRepository.Verify(repository => repository.RemoveParkedVehicleAsync(It.IsAny<Vehicle>()), Times.Once);
            _parkingSpaceRepository.Verify(repository => repository.RemoveParkedVehiclesAsync(It.IsAny<List<ParkingSpace>>()), Times.Once);
            #endregion
        }
    }
}