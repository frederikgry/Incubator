using System;
using System.Threading.Tasks;

namespace Korfinae.Extensions.NamedPipes.Abstractions;


public interface INamedPipeConnection
    : IDisposable
{
    Task SendAsync(string message);
    
    event EventHandler Disconnected; 
    
    event EventHandler<MessageReceivedEventArgs> MessageReceived;
}

public class MessageReceivedEventArgs
    : EventArgs
{
    public MessageReceivedEventArgs(string message)
    {
        Message = message;
    }
    
    public string Message { get; init; }
}







public interface INamedPipeClient
    : INamedPipeConnection
{
    Task ConnectAsync();
    
    event EventHandler ConnectedToServer;
    
    event EventHandler ClientStarted;
}


public interface INamedPipeServer
    : INamedPipeConnection
{
    Task StartAsync();
    
    event EventHandler ServerStarted;
    
    event EventHandler ClientConnected;
}