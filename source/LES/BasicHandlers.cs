using NServiceBus;
using Messages;
using System;
using SagaMessages;

namespace LES
{
    public class FinancalTransactionMessageHandler : IHandleMessages<FinancalTransactionMessage>
    {
        public void Handle(FinancalTransactionMessage message)
        {
            Console.WriteLine("***********************************************************");
            Console.WriteLine("Basic Handler");
            Console.WriteLine("Recieved Financial Transaction");
            Console.WriteLine("***********************************************************");
        }
    }

    public class CheckUnderWriterAgainMessageHandler:IHandleMessages<CheckUnderWriterAgainMessage>
    {
        public void Handle(CheckUnderWriterAgainMessage message)
        {
            Console.WriteLine("***********************************************************");
            Console.WriteLine("Basic Handler"); 
            Console.WriteLine("Recieved Check UnderWriter Again Message");
            Console.WriteLine("***********************************************************");
        }
    }

    public class CheckBrokerAgainMessageHandler : IHandleMessages<CheckBrokerAgainMessage>
    {
        public void Handle(CheckBrokerAgainMessage message)
        {
            Console.WriteLine("***********************************************************");
            Console.WriteLine("Basic Handler"); 
            Console.WriteLine("Recieved Check Broker Again Message");
            Console.WriteLine("***********************************************************");
        }
    }
}
