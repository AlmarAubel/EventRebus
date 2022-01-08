// See https://aka.ms/new-console-template for more information

using EventRebus;
using Rebus.Activation;
using Rebus.Config;
using Rebus.DataBus.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

var network = new InMemNetwork();
var destinations = new[] {"queue1", "queue3"};
 
var m1 = new Module1().Init(network, "1",destinations);
var m2 = new Module2().Init(network, "3",destinations);
var m3 = new Module2().Init(network, "4",destinations);
while (true)
{
    await m1.send();
    Thread.Sleep(4000);
    Console.WriteLine("....");
    //await m2.send();
}