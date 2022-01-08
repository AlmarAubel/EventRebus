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

public class Publisher
{
    private ServiceCollection _services;
    private BuiltinHandlerActivator _activator1;

    public Publisher Init(InMemNetwork inMemNetwork, string[] queues)
    {
        _services = new ServiceCollection();

        _activator1 = new BuiltinHandlerActivator();
        var queueName = "publisher"; // start bus 1

        Configure.With(_activator1)
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queueName ))
            .Logging(t => t.ColoredConsole())
            .Subscriptions(s=> s.StoreInMemory())
            .Start();
        
        return this;
    }

    public async Task send()
    {
        await _activator1.Bus.Publish(new MessageEvent("test"));
    }
}