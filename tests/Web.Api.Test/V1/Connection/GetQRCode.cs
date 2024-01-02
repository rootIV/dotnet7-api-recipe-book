using Moq;
using MyRecipeBook.Api.WebSockets;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using Web.Api.Test.V1.Connection.Builder;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class GetQRCode
{
    //[Fact]
    //public async Task Validate_Success()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var GenerateQRCodeUseCase = GenerateQRCodeUseCaseBuilder(generatedCodeToConnection);

    //    var hub = new AddConnection(hubContextMock.Object, GenerateQRCodeUseCase, null, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("ResultQRCode",
    //        It.Is<object[]>(response => response != null 
    //        && response.Length == 1 
    //        && response.First().Equals(generatedCodeToConnection)),
    //        default),
    //        Times.Once);
    //}
    //[Fact]
    //public async Task Validate_Unknown_Erro()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var GenerateQRCodeUseCase = GenerateQRCodeUseCase_UnknowErroBuilder();

    //    var hub = new AddConnection(hubContextMock.Object, GenerateQRCodeUseCase, null, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("Erro",
    //        It.Is<object[]>(response => response != null 
    //        && response.Length == 1 
    //        && response.First().Equals(ErroMessagesResource.Unknown_Error)),
    //        default),
    //        Times.Once);
    //}
    //[Fact]
    //public async Task Validate_MyRecipeBook_Erro()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var GenerateQRCodeUseCase = GenerateQRCodeUseCase_MyRecipeBookExceptionBuilder(ErroMessagesResource.User_Unauthorized_Operation);

    //    var hub = new AddConnection(hubContextMock.Object, GenerateQRCodeUseCase, null, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("Erro",
    //        It.Is<object[]>(response => response != null 
    //        && response.Length == 1 
    //        && response.First().Equals(ErroMessagesResource.User_Unauthorized_Operation)),
    //        default),
    //        Times.Once);
    //}

    //private static IGenerateQRCodeUseCase GenerateQRCodeUseCaseBuilder(string generatedCodeToConnection)
    //{
    //    var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ReturnsAsync((generatedCodeToConnection, "userId"));

    //    return useCaseMock.Object;
    //}
    //private static IGenerateQRCodeUseCase GenerateQRCodeUseCase_UnknowErroBuilder()
    //{
    //    var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ThrowsAsync(new ArgumentNullException());

    //    return useCaseMock.Object;
    //}
    //private static IGenerateQRCodeUseCase GenerateQRCodeUseCase_MyRecipeBookExceptionBuilder(string erroMessage)
    //{
    //    var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ThrowsAsync(new MyRecipeBookException(erroMessage));

    //    return useCaseMock.Object;
    //}
}
