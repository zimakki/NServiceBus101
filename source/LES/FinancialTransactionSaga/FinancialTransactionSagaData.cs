using System;
using NServiceBus.Saga;

namespace LES.FinancialTransactionSaga
{
    public class FinancialTransactionSagaData : ISagaEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Originator { get; set; }
        public virtual string OriginalMessageId { get; set; }
        public virtual Guid SagaId { get; set; }
        public virtual bool UnderwriterExists { get; set; }
        public virtual bool BrokerExists { get; set; }
        public virtual DateTime SagaStartTime { get; set; }
    }
}