using NServiceBus;

namespace FakeMessagePublisher
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher
    {
    }
}
