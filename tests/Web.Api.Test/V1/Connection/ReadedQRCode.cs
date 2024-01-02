using Moq;
using MyRecipeBook.Api.WebSockets;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Responses;
using Web.Api.Test.V1.Connection.Builder;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class ReadedQRCode
{
    //[Fact]
    //public async Task Validate_Success()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var ReadedQRCodeUseCase = ReadedQRCodeUseCaseBuilder(userToConnect, generatedCodeToConnection);
    //    var GenerateQrCodeUseCase = GetQRCodeUseCaseBuilder(generatedCodeToConnection);

    //    var hub = new AddConnection(hubContextMock.Object, GenerateQrCodeUseCase, ReadedQRCodeUseCase, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.ReadedQRCode(generatedCodeToConnection);

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("ReadedQRCode",
    //        It.Is<object[]>(response => response != null 
    //        && response.Length == 1 
    //        && (response.First() as ResponseUserConnectionJson).Name.Equals(userToConnect.Name) 
    //        && (response.First() as ResponseUserConnectionJson).Id.Equals(userToConnect.Id)),
    //        default), Times.Once);
    //}
    //[Fact]
    //public async Task Validate_Unknown_Erro()
    //{
    //    var generatedCodeToConnection = Guid.NewGuid().ToString();
    //    var userToConnect = ResponseUserConnectionBuilder.Build();

    //    (var hubContextMock, var clientProxyMock, var clientsMock, var hubContextCallerMock) = MockWebSocketConnectionBuilder.Build();

    //    var ReadedQRCodeUseCase = ReadedQRCodeUseCase_UnknownErroBuilder(generatedCodeToConnection);
    //    var GenerateQrCodeUseCase = GetQRCodeUseCaseBuilder(generatedCodeToConnection);

    //    var hub = new AddConnection(hubContextMock.Object, GenerateQrCodeUseCase, ReadedQRCodeUseCase, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.ReadedQRCode(generatedCodeToConnection);

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

    //    var ReadedQRCodeUseCase = ReadedQRCodeUseCase_MyRecipeBookExceptionBuilder(generatedCodeToConnection, ErroMessagesResource.User_Not_Found);


    //    var hub = new AddConnection(hubContextMock.Object, null, ReadedQRCodeUseCase, null, null)
    //    {
    //        Context = hubContextCallerMock.Object,
    //        Clients = clientsMock.Object
    //    };

    //    await hub.GenerateQRCode();

    //    await hub.ReadedQRCode(generatedCodeToConnection);

    //    clientProxyMock.Verify(clientProxy => clientProxy.SendCoreAsync("Erro",
    //        It.Is<object[]>(response => response != null
    //        && response.Length == 1
    //        && response.First().Equals(ErroMessagesResource.User_Not_Found)),
    //        default),
    //        Times.Once);
    //}

    //private static IGenerateQRCodeUseCase GetQRCodeUseCaseBuilder(string qrCode)
    //{
    //    var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute()).ReturnsAsync((qrCode, "userId"));

    //    return useCaseMock.Object;
    //}
    //private static IReadedQRCodeUseCase ReadedQRCodeUseCaseBuilder(ResponseUserConnectionJson jsonResponse, string qrCode)
    //{
    //    var useCaseMock = new Mock<IReadedQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute(qrCode)).ReturnsAsync((jsonResponse, "userId"));

    //    return useCaseMock.Object;
    //}
    //private static IReadedQRCodeUseCase ReadedQRCodeUseCase_UnknownErroBuilder(string qrCode)
    //{
    //    var useCaseMock = new Mock<IReadedQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute(qrCode)).ThrowsAsync(new ArgumentNullException(String.Empty));

    //    return useCaseMock.Object;
    //}
    //private static IReadedQRCodeUseCase ReadedQRCodeUseCase_MyRecipeBookExceptionBuilder(string qrCode, ResponseUserConnectionJson jsonResponse)
    //{
    //    var useCaseMock = new Mock<IReadedQRCodeUseCase>();

    //    useCaseMock.Setup(c => c.Execute(qrCode)).ReturnsAsync((jsonResponse, "invalidId"));

    //    return useCaseMock.Object;
    //}
}
