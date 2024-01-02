using Moq;
using MyRecipeBook.Api.WebSockets;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Application.UseCases.Connection.RefuseConnection;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Responses;
using Web.Api.Test.V1.Connection.Builder;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class RefuseConnection
{
    //[Fact]
    //public async Task Validate_Success()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var generateQRCodeUseCase = GenerateQRCodeUseCaseBuilder(generatedCodeToConnection);
    //    var refuseConnectionUseCase = RefuseConnectionUseCaseBuilder();

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, refuseConnectionUseCase, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.RefuseConnection();

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("OnRefusedConnection", 
    //        It.Is<object[]>(response => response != null && response.Length == 0), 
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
    //    var refuseConnectionUseCase = RefuseConnectionUseCase_UnkownErroBuilder();

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, refuseConnectionUseCase, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.RefuseConnection();

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
    //    var refuseConnectionUseCase = RefuseConnectionUseCaseBuilder();

    //    var hub = new AddConnection(hubContextMock.Object, generateQRCodeUseCase, null, refuseConnectionUseCase, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.RefuseConnection();

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
    //private static IRefuseConnectionUseCase RefuseConnectionUseCaseBuilder()
    //{
    //    var useCaseMock = new Mock<IRefuseConnectionUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ReturnsAsync("userId");

    //    return useCaseMock.Object;
    //}
    //private static IRefuseConnectionUseCase RefuseConnectionUseCase_UnkownErroBuilder()
    //{
    //    var useCaseMock = new Mock<IRefuseConnectionUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ThrowsAsync(new ArgumentException());

    //    return useCaseMock.Object;
    //}
}
