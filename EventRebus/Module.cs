using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;

using Rebus.Handlers;
using Rebus.Persistence.FileSystem;
using Rebus.Persistence.InMem;
using Rebus.Routing.TransportMessages;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace EventRebus;

public class Publisher
{
    private ServiceCollection _services;
    private ServiceProvider _provider;

    public Publisher Init(InMemNetwork inMemNetwork, string[] queues)
    {
        _services = new ServiceCollection();
        var queueName = "publisher"; // start bus 1
        
        _services.AddRebus(config => config
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queueName))
            .Logging(t => t.ColoredConsole())
            .Subscriptions(s => s.StoreInMemory())
        );
        
        _provider = _services.BuildServiceProvider();
        _provider.UseRebus();
        return this;
    }

    public async Task send()
    {
        var bus = _provider.GetRequiredService<IBus>();
        await bus.Publish(new MessageEvent("test"));
    }
}