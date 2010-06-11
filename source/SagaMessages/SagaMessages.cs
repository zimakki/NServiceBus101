using System;
using NServiceBus;

namespace SagaMessages
{
    public class CheckUnderWriterAgainMessage : IMessage
    {
        public Guid SagaId { get; set; }
    }
}
