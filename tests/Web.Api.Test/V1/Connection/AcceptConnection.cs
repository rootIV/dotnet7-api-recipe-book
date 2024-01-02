using Moq;
using MyRecipeBook.Api.WebSockets;
using MyRecipeBook.Application.UseCases.Connection.AcceptConnection;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Responses;
using Web.Api.Test.V1.Connection.Builder;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class AcceptConnection
{
    //[Fact]
    //public async Task Validate_Success()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var generateQRCodeUseCase = GenerateQRCodeUseCaseBuilder(generatedCodeToConnection);
    //    var acceptConnectionUseCase = AcceptQRCodeUseCaseBuilder(userToConnect.Id);

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, null, acceptConnectionUseCase)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.AcceptConnection(userToConnect.Id);

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("OnAcceptedConnection",
    //        It.Is<object[]>(response => response != null 
    //        && response.Length == 0),
    //        default),
    //        Times.Once);
    //}
    //[Fact]
    //public async Task Validate_Unknown_Erro()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var generateQRCodeUseCase = GenerateQRCodeUseCaseBuilder(generatedCodeToConnection);
    //    var acceptConnectionUseCase = AcceptConnectionUseCase_UnkownErroBuilder(userToConnect.Id);

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, null, acceptConnectionUseCase)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.AcceptConnection(userToConnect.Id);

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("Erro",
    //        It.Is<object[]>(response => response != null
    //        && response.Length == 1
    //        && response.First().Equals(ErroMessagesResource.Unknown_Error)),
    //        default),
    //        Times.Once);
    //}
    //[Fact]
    //public async Task Validate_MyRecipeBookException_Erro()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var generateQRCodeUseCase = GenerateQRCodeUseCaseBuilder(generatedCodeToConnection);
    //    var acceptConnectionUseCase = AcceptQRCodeUseCaseBuilder(userToConnect.Id);

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, null, acceptConnectionUseCase)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    userToConnect.Id = Guid.NewGuid().ToString();

    //    await hub.AcceptConnection(userToConnect.Id);

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("Erro",
    //        It.Is<object[]>(response => response != null
    //        && response.Length == 1
    //        && response.First().Equals(ErroMessagesResource.User_Not_Found)),
    //        default),
    //        Times.Once);
    //}

    //private static IGenerateQRCodeUseCase GenerateQRCodeUseCaseBuilder(string qrCode)
    //{
    //    var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ReturnsAsync((qrCode, "userId"));

    //    return useCaseMock.Object;
    //}
    //private static IAcceptConnectionUseCase AcceptQRCodeUseCaseBuilder(string userToConnectId)
    //{
    //    var useCaseMock = new Mock<IAcceptConnectionUseCase>();

    //    useCaseMock.Setup(c => c.Execute(userToConnectId)).ReturnsAsync("userId");

    //    return useCaseMock.Object;
    //}
    //private static IAcceptConnectionUseCase AcceptConnectionUseCase_UnkownErroBuilder(string userToConnectId)
    //{
    //    var useCaseMock = new Mock<IAcceptConnectionUseCase>();

    //    useCaseMock.Setup(c => c.Execute(userToConnectId)).ThrowsAsync(new ArgumentException(String.Empty));

    //    return useCaseMock.Object;
    //}
}
