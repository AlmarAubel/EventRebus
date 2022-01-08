using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Config;

using Rebus.Handlers;
using Rebus.Persistence.FileSystem;
using Rebus.Persistence.InMem;
using Rebus.Routing.TransportMessages;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace EventRebus;

public class Module1
{
    private ServiceCollection _services;
    private BuiltinHandlerActivator _activator1;

    public Module1 Init(InMemNetwork inMemNetwork, string s, string[] queues)
    {
        _services = new ServiceCollection();

        _activator1 = new BuiltinHandlerActivator();
        var queuenname = "publisher"; // start bus 1

        var JsonFilePath = "./subscribers.json";
        Configure.With(_activator1)
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queuenname ))
            .Logging(t => t.ColoredConsole())
            .Subscriptions(s=> s.StoreInMemory())
            .Start();
        
        return this;
    }

    public async Task send()
    {
        await _activator1.Bus.Publish(new WeatherEvent("test"));
    }
}

public class WeatherEvent
{
    public WeatherEvent(string message)
    {
        Message = message;
    }
    public string  Message { get; set; }
    
} 
