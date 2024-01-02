using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyRecipeBook.Application.UseCases.Connection.AcceptConnection;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;
using MyRecipeBook.Application.UseCases.Connection.RefuseConnection;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Api.WebSockets;

[Authorize(Policy = "LoggedUser")]
public class AddConnection : Hub
{
    private readonly Broadcaster _broadcaster;
    private readonly IGenerateQRCodeUseCase _generateQRCodeUseCase;
    private readonly IHubContext<AddConnection> _hubContext;
    private readonly IReadedQRCodeUseCase _readedQRCodeUseCase;
    private readonly IRefuseConnectionUseCase _refuseConnectionUseCase;
    private readonly IAcceptConnectionUseCase _acceptConnectionUseCase;

    public AddConnection(IHubContext<AddConnection> hubContext,
        IGenerateQRCodeUseCase generateQRCodeUseCase,
        IReadedQRCodeUseCase readedQRCodeUseCase,
        IRefuseConnectionUseCase refuseConnectionUseCase,
        IAcceptConnectionUseCase acceptConnectionUseCase)
    {
        _broadcaster = Broadcaster.Instance;
        _generateQRCodeUseCase = generateQRCodeUseCase;
        _hubContext = hubContext;
        _readedQRCodeUseCase = readedQRCodeUseCase;
        _refuseConnectionUseCase = refuseConnectionUseCase;
        _acceptConnectionUseCase = acceptConnectionUseCase;
    }

    public async Task GenerateQRCode()
    {
        try
        {
            (var qrCode, var userId) = await _generateQRCodeUseCase.Execute();

            _broadcaster.InitializeConnection(_hubContext, userId, Context.ConnectionId);

            await Clients.Caller.SendAsync("ResultQRCode", qrCode);
        }
        catch (MyRecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ErroMessagesResource.Unknown_Error);
        }
    }
    public async Task ReadedQRCode(string connectionCode)
    {
        try
        {
            (var ConnectingUser, var QRCodeGeneratorUserId) = await _readedQRCodeUseCase.Execute(connectionCode);

            var connectionId = _broadcaster.GetConnectionUserId(QRCodeGeneratorUserId);

            _broadcaster.ExpirationTimeReset(connectionId);
            _broadcaster.SetQRCodeUserReader(QRCodeGeneratorUserId, Context.ConnectionId);

            await Clients.Client(connectionId).SendAsync("ReadedQRCode", ConnectingUser);
        }
        catch (MyRecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ErroMessagesResource.Unknown_Error);
        }
    }
    public async Task RefuseConnection()
    {
        try
        {
            var QRCodeGeneratorUserId = Context.ConnectionId;

            var userId = await _refuseConnectionUseCase.Execute();

            var qrCodeReaderConnectionId = _broadcaster.Remove(QRCodeGeneratorUserId, userId);

            await Clients.Client(qrCodeReaderConnectionId).SendAsync("OnRefusedConnection");
        }
        catch (MyRecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ErroMessagesResource.Unknown_Error);
        }
    }
    public async Task AcceptConnection(string userToConnectId)
    {
        try
        {
            var userId = await _acceptConnectionUseCase.Execute(userToConnectId);

            var qrCodeGeneratorConnectionId = Context.ConnectionId;

            var qrCodeReaderConnectionId = _broadcaster.Remove(qrCodeGeneratorConnectionId, userId);

            await Clients.Client(qrCodeReaderConnectionId).SendAsync("OnAcceptedConnection");
        }
        catch (MyRecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ErroMessagesResource.Unknown_Error);
        }
    }
}
