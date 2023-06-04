using AutoMapper;
using Bridge.Application.Common.Interfaces;
using Bridge.Application.Dogs.Commands.CreateNewDog;
using Bridge.Domain.Entities.Dog;
using Bridge.Domain.Repositories;

namespace Bridge.UnitTests;

public class DogsUnitTests
{
    [Fact]
    public async Task CreateNewDogCommandHandler_Throws_WhenDogNameAlreadExists()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<IDogRepository>();
        repoMock.Setup(x => x.IsNameUnique(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateNewDogCommand("Dummy name", "Dummy color", 0, 0);
        var handler = new CreateNewDogCommandHandler(mapperMock.Object, 
            unitOfWorkMock.Object, repoMock.Object);
        
        await Assert.ThrowsAsync<InvalidDataException>(async () => await handler.Handle(command, default));
    }

    [Fact]
    public async Task CreateNewDogCommandHandler_CreatesNewDog_WhenNameIsUnique()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<IDogRepository>();
        repoMock.Setup(x => x.IsNameUnique(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateNewDogCommand("Dummy name", "Dummy color", 0, 0);
        mapperMock.Setup(x => x.Map<Dog>(command))
            .Returns(new Dog());
        var handler = new CreateNewDogCommandHandler(mapperMock.Object,
            unitOfWorkMock.Object, repoMock.Object);

        await handler.Handle(command, default);
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}  