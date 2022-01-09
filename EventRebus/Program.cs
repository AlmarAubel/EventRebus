// See https://aka.ms/new-console-template for more information

using EventRebus;
using Rebus.Activation;
using Rebus.Config;
using Rebus.DataBus.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

var network = new InMemNetwork();
var destinations = new[] {"queue1", "queue3"};
var consumer1 = new Consumer().Init(network, "1",destinations);
//var consumer2 = new Consumer().Init(network, "2",destinations);
var publisher = new Publisher().Init(network ,destinations);

while (true)
{
    await publisher.send();
    await Task.Delay(3000);
    Console.WriteLine("....");
    
    
}