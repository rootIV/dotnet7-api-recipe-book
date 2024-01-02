using Microsoft.AspNetCore.SignalR;
using Moq;
using MyRecipeBook.Api.WebSockets;

namespace Web.Api.Test.V1.Connection.Builder;

public class MockWebSocketConnectionBuilder
{
    public static (Mock<IHubContext<AddConnection>> hubContextMock, Mock<ISingleClientProxy> clientProxyMock, Mock<IHubCallerClients> clientsMock, Mock<HubCallerContext> hubContextCallerMock) Build()
    {
        var hubClientsMock = new Mock<IHubClients>();

        var clientProxyMock = new Mock<ISingleClientProxy>();

        hubClientsMock.Setup(c => c.Client(It.IsAny<string>())).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<AddConnection>>();
        hubContextMock.Setup(c => c.Clients).Returns(hubClientsMock.Object);

        var hubContextCallerMock = new Mock<HubCallerContext>();
        hubContextCallerMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString());

        var clientsMock = new Mock<IHubCallerClients>();
        clientsMock.Setup(c => c.Caller).Returns(clientProxyMock.Object);
        clientsMock.Setup(c => c.Client(It.IsAny<string>())).Returns(clientProxyMock.Object);

        return (hubContextMock, clientProxyMock, clientsMock, hubContextCallerMock);
    }
}
