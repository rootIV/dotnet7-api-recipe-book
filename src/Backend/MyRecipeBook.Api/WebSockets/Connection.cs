using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace MyRecipeBook.Api.WebSockets;

public class Connection
{
    private readonly IHubContext<AddConnection> _hubContext;
    private readonly string _qRCodeConnectionIdOwnerUser;
    private Action<string> _callbackExpiredTime;
    private string _qRCodeConnectionIdUserReader;

    public Connection(IHubContext<AddConnection> hubContext, string qRCodeConnectionIdOwnerUser)
    {
        _hubContext = hubContext;
        _qRCodeConnectionIdOwnerUser = qRCodeConnectionIdOwnerUser;
    }

    private short RemainTimeInSeconds { get; set; }
    private System.Timers.Timer Timer { get; set; }

    public void InitializaTimeCounting(Action<string> callbackExpiredTime)
    {
        _callbackExpiredTime = callbackExpiredTime;

        StartTimer();
    }
    public void StopTimer()
    {
        Timer?.Stop();
        Timer?.Dispose();
        Timer = null;
    }
    public void TimeCountingReset()
    {
        StopTimer();
        StartTimer();
    }
    public string GetQRCodeUserReader()
    {
        return _qRCodeConnectionIdUserReader;
    }
    public void SetQRCodeUserReader(string connectionId)
    {
        _qRCodeConnectionIdUserReader = connectionId;
    }

    private async void ElapsedTimer(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        if (RemainTimeInSeconds >= 0)
            await _hubContext.Clients.Client(_qRCodeConnectionIdOwnerUser).SendAsync("SetRemainTime", RemainTimeInSeconds--);
        else
        {
            StopTimer();
            _callbackExpiredTime(_qRCodeConnectionIdOwnerUser);
        }
    }
    private void StartTimer()
    {
        RemainTimeInSeconds = 60;

        Timer = new System.Timers.Timer(1000)
        {
            Enabled = true
        };

        Timer.Elapsed += ElapsedTimer;
        Timer.Enabled = true;
    }
}
