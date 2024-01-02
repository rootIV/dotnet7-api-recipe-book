using Microsoft.AspNetCore.SignalR;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using System.Collections.Concurrent;

namespace MyRecipeBook.Api.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());
    private ConcurrentDictionary<string, object> Dictionary { get; set; }

    public static Broadcaster Instance { get { return _instance.Value; } }

 
    public Broadcaster()
    {
        Dictionary = new();
    }

    public void InitializeConnection(IHubContext<AddConnection> hubContext, 
        string QRCodeGeneratorUserId, 
        string connectionId)
    {
        var connection = new Connection(hubContext, connectionId);

        Dictionary.TryAdd(connectionId, connection);
        Dictionary.TryAdd(QRCodeGeneratorUserId, connectionId);

        connection.InitializaTimeCounting(ExpiredTimeCallback);
    }
    public string GetConnectionUserId(string userId)
    {
        if(!Dictionary.TryGetValue(userId, out var connectionId))
        {
            throw new MyRecipeBookException(ErroMessagesResource.User_Not_Found);
        }

        return connectionId.ToString();
    }
    public void ExpirationTimeReset(string connectionId)
    {
        Dictionary.TryGetValue(connectionId, out var connectionObject);

        var connection = connectionObject as Connection;

        connection.TimeCountingReset();
    }
    public string Remove(string connectionId, string userId)
    {
        if (!Dictionary.TryGetValue(connectionId, out var connectionObject))
        {
            throw new MyRecipeBookException(ErroMessagesResource.User_Not_Found);
        }

        var connection = connectionObject as Connection;
        connection.StopTimer();

        Dictionary.TryRemove(connectionId, out _);
        Dictionary.TryRemove(userId, out _);

        return connection.GetQRCodeUserReader();
    }
    public void SetQRCodeUserReader(string qrCodeGeneratorConnectionId, string qrCodeReaderConnectionId)
    {
        var qrCodeReaderUserId = GetConnectionUserId(qrCodeGeneratorConnectionId);
        Dictionary.TryGetValue(qrCodeReaderUserId, out var connectionObject);

        var connection = connectionObject as Connection;

        connection.SetQRCodeUserReader(qrCodeReaderConnectionId);
    }

    private void ExpiredTimeCallback(string connectionId)
    {
        Dictionary.TryRemove(connectionId, out _);
    }
}
