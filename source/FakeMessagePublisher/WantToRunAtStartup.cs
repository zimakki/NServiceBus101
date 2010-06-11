using NServiceBus;
using SagaMessages;

namespace FakeMessagePublisher
{
    using System;

    using Messages;

    class WantToRunAtStartup : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {

            Console.WriteLine("***************************************Run***************************************");

            while (true)
            {
                Console.WriteLine("f --> FinancialTransactionMessage\n B --> CheckBrokerAgainMessage\n u --> CheckUnderWriterAgainMessage");

                IMessage message = GetFinancialTransactionMessage();
                switch (Console.ReadLine())
                {
                    case "u":
                        message = new CheckUnderWriterAgainMessage { SagaId = Guid.NewGuid() };
                        break;
                    case "b":
                        message = new CheckBrokerAgainMessage { SagaId = Guid.NewGuid() };
                    defaul:
                        break;
                }
                
                Bus.Publish(message);
                Console.WriteLine("Message published");
                
            }
        }

        private IMessage GetFinancialTransactionMessage()
        {
            return new FinancalTransactionMessage
                       {
                           BrokerId = "BrokerId", 
                           PolicyReference = "PolicyReference", 
                           UnderwriterId = "UnderwriterId",
                           SagaId = Guid.NewGuid(),
                           SagaStartTime = DateTime.Now
                       };
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
