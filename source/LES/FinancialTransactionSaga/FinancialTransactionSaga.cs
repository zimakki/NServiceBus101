using System;
using System.Collections.Generic;
using LES.DataAccess;
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
        private string checkBrokerAgainMessageState = "checkBrokerAgainMessage";
        private string checkUnderWriterAgainMessageState = "checkUnderWriterAgainMessage";

        public override void ConfigureHowToFindSaga()
        {
            Console.WriteLine(new List<string> { "ConfigureHowToFindSaga" });
            ConfigureMapping<FinancalTransactionMessage>(saga => saga.SagaId, message => message.SagaId);
            ConfigureMapping<CheckUnderWriterAgainMessage>(saga => saga.SagaId, message => message.SagaId);
            ConfigureMapping<CheckBrokerAgainMessage>(saga => saga.SagaId, message => message.SagaId);
        }

        public void Handle(FinancalTransactionMessage message)
        {
            Console.WriteLine(new List<string> { "In Saga", "Handle(FinancalTransactionMessage message)" });

            Data.SagaId = message.SagaId;
            Data.SagaStartTime = message.SagaStartTime;
            Data.BrokerExists = EntityChecker.BrokerExists();
            Data.UnderwriterExists = EntityChecker.UnderwriterExists();

            if (Data.BrokerExists && Data.UnderwriterExists)
            {
                PersistFinancialTransation();
                CompleteSaga();
            }

            if (Data.BrokerExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), checkBrokerAgainMessageState);

            if (Data.UnderwriterExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), checkUnderWriterAgainMessageState);
        }

        public void Handle(CheckUnderWriterAgainMessage message)
        {
            Console.WriteLine(new List<string> { "In Saga", "public void Handle(CheckUnderWriterAgainMessage message)" });
            Data.BrokerExists = EntityChecker.BrokerExists();
            if (Data.BrokerExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), checkBrokerAgainMessageState);
        
        }

        public void Handle(CheckBrokerAgainMessage message)
        {
            Console.WriteLine(new List<string> { "In Saga", "public void Handle(CheckBrokerAgainMessage message)" });
            Data.UnderwriterExists = EntityChecker.UnderwriterExists();
            if (Data.UnderwriterExists == false)
                RequestTimeout(TimeSpan.FromSeconds(3), checkUnderWriterAgainMessageState);
        }

        public override void Timeout(object state)
        {
            Console.WriteLine(new List<string> { "In the timeout" });

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

        private void CompleteSaga()
        {
            MarkAsComplete();
            Console.WriteLine(new List<string> { "Finished!!!! :)" });
        }

        private static void PersistFinancialTransation()
        {
            Console.WriteLine(new List<string> { "PersistFinancialTransation" });
        }

        
    }
}
