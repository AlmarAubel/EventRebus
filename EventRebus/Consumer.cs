using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Config;
using Rebus.DataBus.InMem;
using Rebus.Handlers;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace EventRebus;

public class Consumer
{
    private ServiceCollection _services;
    private ServiceProvider _provider;

    public Consumer Init(InMemNetwork inMemNetwork,  string s, string[] queues)
    {
        _services = new ServiceCollection();
        
        var queuenname = $"Consumer-{s}"; // start bus 1
      
        _services.AddRebus(config => config
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queuenname))
            .Logging(t => t.ColoredConsole())
            .Routing(x => x.TypeBased().MapAssemblyOf<MessageEvent>("publisher"))
        );
        
        _services.AutoRegisterHandlersFromAssemblyOf<MessageEvent>();

        _provider = _services.BuildServiceProvider();
        _provider.UseRebus(x=> x.Subscribe<MessageEvent>());
        
        return this;
    }
}

class MessageEventHandler : IHandleMessages<MessageEvent>
{
    public async Task Handle(MessageEvent message)
    {
        Console.WriteLine($"MessageEventHandler MSG: {message.Message}");
    }
}