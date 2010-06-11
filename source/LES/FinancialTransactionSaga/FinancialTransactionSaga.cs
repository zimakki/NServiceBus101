using System;
using System.Collections.Generic;
using Messages;
using NServiceBus;
using NServiceBus.Saga;
using SagaMessages;

namespace LES.FinancialTransactionSaga
{
    class FinancialTransactionSaga : Saga<FinancialTransactionSagaData>,
        IAmStartedByMessages<FinancalTransactionMessage>,
        IHandleMessages<CheckUnderWriterAgainMessage>,
        IHandleMessages<CheckBrokerAgainMessage>
    {
        public override void ConfigureHowToFindSaga()
        {
            WriteOut(new List<string> { "ConfigureHowToFindSaga" });
            ConfigureMapping<FinancalTransactionMessage>(saga => saga.SagaId, message => message.SagaId);
            ConfigureMapping<CheckUnderWriterAgainMessage>(saga => saga.SagaId, message => message.SagaId);
            ConfigureMapping<CheckBrokerAgainMessage>(saga => saga.SagaId, message => message.SagaId);
        }

        public void Handle(FinancalTransactionMessage message)
        {
            WriteOut(new List<string>{"In Saga","Handle(FinancalTransactionMessage message)"});

            Data.SagaId = message.SagaId;
            Data.SagaStartTime = message.SagaStartTime;
            Data.BrokerExists = CheckBrokerExisters();
            Data.UnderwriterExists = CheckUnderwriterExisters();

            if (Data.BrokerExists && Data.UnderwriterExists){/*save out to the Excellerator table*/}

            if (Data.BrokerExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), "checkBrokerAgainMessage");

            if (Data.UnderwriterExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), "checkUnderWriterAgainMessage");
        }

        public void Handle(CheckUnderWriterAgainMessage message){WriteOut(new List<string> { "public void Handle(CheckUnderWriterAgainMessage message)" });}

        public void Handle(CheckBrokerAgainMessage message){WriteOut(new List<string> { "public void Handle(CheckBrokerAgainMessage message)" });}

        public override void Timeout(object state)
        {
            WriteOut(new List<string> { "In the timeout" });

            if (Data.UnderwriterExists == false)
            {
                var checkUnderWriterAgainMessage = new CheckUnderWriterAgainMessage { SagaId = Data.SagaId };
                Bus.Send(checkUnderWriterAgainMessage);
            }

            if (Data.BrokerExists == false)
            {
                var checkBrokerAgainMessage = new CheckBrokerAgainMessage { SagaId = Data.SagaId };
                Bus.Send(checkBrokerAgainMessage);
            }
        }

        private bool CheckBrokerExisters()
        {
            WriteOut(new List<string> { "Call to CheckBrokerExisters" });
            return true;
        }

        private bool CheckUnderwriterExisters()
        {
            WriteOut(new List<string> { "Call to CheckUnderwriterExisters" });
            return false;
        }

        private static void WriteOut(IEnumerable<string> messages)
        {
            Console.WriteLine("************************************************************************************************");
            foreach (var currentMessage in messages)
            {
                Console.WriteLine(currentMessage);
            }
            Console.WriteLine("************************************************************************************************");
        }
    }

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
