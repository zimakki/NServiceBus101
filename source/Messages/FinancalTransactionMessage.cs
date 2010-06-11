using System;
using NServiceBus;

namespace Messages
{
    public class FinancalTransactionMessage : IMessage
    {
        public string PolicyReference { get; set; }
        public string UnderwriterId { get; set; }
        public string BrokerId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime SagaStartTime { get; set; }
    }
}
