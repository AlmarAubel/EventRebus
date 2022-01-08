using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Config;
using Rebus.DataBus.InMem;
using Rebus.Handlers;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace EventRebus;

public class Module2
{
    private ServiceCollection _services;
    private BuiltinHandlerActivator _activator;

    public Module2 Init(InMemNetwork inMemNetwork,  string s, string[] queues)
    {
        _services = new ServiceCollection();

        _activator = new BuiltinHandlerActivator();
       
        var queuenname = "Consumer"+s; // start bus 1
        _activator.Register(() => new Handler1(queuenname));

        Configure.With(_activator)
            .Transport(t => t.UseInMemoryTransport(inMemNetwork, queuenname ))
            .Logging(t => t.ColoredConsole())
            .Routing(x=> x.TypeBased().MapAssemblyOf<WeatherEvent>("publisher"))
            .Start();
        
        _activator.Bus.Subscribe<WeatherEvent>().Wait();
        return this;
    }
}

class Handler1 : IHandleMessages<WeatherEvent>
{
    private string _queueName;

    public Handler1(string queueName)
    {
        _queueName = queueName;
    }

    public async Task Handle(WeatherEvent message)
    {
        Console.WriteLine("Handler1 Handler: " + _queueName + " MSG: " + message.Message);
    }
}