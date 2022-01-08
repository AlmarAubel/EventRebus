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
    private BuiltinHandlerActivator _activator;

    public Consumer Init(InMemNetwork inMemNetwork,  string s, string[] queues)
    {
        _services = new ServiceCollection();

        _activator = new BuiltinHandlerActivator();
       
        var queuenname = $"Consumer-{s}"; // start bus 1
        _activator.Register(() => new MessageEventHandler(queuenname));

        Configure.With(_activator)
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queuenname ))
            .Logging(t => t.ColoredConsole())
            .Routing(x=> x.TypeBased().MapAssemblyOf<MessageEvent>("publisher"))
            .Start();
        
        _activator.Bus.Subscribe<MessageEvent>().Wait();
        return this;
    }
}

class MessageEventHandler : IHandleMessages<MessageEvent>
{
    private string _queueName;

    public MessageEventHandler(string queueName)
    {
        _queueName = queueName;
    }

    public async Task Handle(MessageEvent message)
    {
        Console.WriteLine("MessageEventHandler Handler: " + _queueName + " MSG: " + message.Message);
    }
}