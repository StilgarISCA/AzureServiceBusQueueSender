using System;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace ServiceBusTest
{
   public static class Program
   {
      private static readonly string _sbConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
      private static readonly string _sbDestinationQueue = ConfigurationManager.AppSettings["DestinationQueue"];
      private static readonly string _userId = Guid.NewGuid().ToString();

      public static void Main( string[] args )
      {
         QueueClient client = QueueClient.CreateFromConnectionString( _sbConnectionString, _sbDestinationQueue );

         Console.Write( "Composing message..." );
         var brokeredMessage = new BrokeredMessage( "Message body goes here." );

         // You can set properties as key/value pairs
         brokeredMessage.Properties["_userId"] = _userId;
         Console.Write( "done.\n\n" );

         string msgStatus;

         Console.Write( "Sending message..." );
         try
         {
            client.Send( brokeredMessage );
            msgStatus = "sent";
         }
         catch ( MessagingEntityNotFoundException )
         {
            msgStatus = "failed: Messaging Entity Not Found";
         }

         client.Close();

         Console.WriteLine( "{0}.\n\nPress enter to quit.\n", msgStatus );
         Console.ReadLine();
      }
   }
}
