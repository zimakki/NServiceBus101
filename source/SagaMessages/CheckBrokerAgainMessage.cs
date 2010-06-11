using System;
using NServiceBus;

namespace SagaMessages
{
    public class CheckBrokerAgainMessage : IMessage
    {
        public Guid SagaId { get; set; }
    }
}