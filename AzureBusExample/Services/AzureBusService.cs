using AzureBusExample.Models;
using System.Text.Json;
using System.Text;
using Microsoft.Azure.ServiceBus;

namespace AzureBusExample.Services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IConfiguration _config;

        public AzureBusService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            // Get the connection string from the settings
            var connectionString = _config.GetConnectionString("AzureServiceBus");

            // Initializing the Queue
            var qClient = new QueueClient(connectionString, queueName);

            // convert the person obj into json
            var msgBody = JsonSerializer.Serialize(personMessage);

            // Initialize the Queue message
            var msg = new Message(Encoding.UTF8.GetBytes(msgBody));

            // Sent the message
            await qClient.SendAsync(msg);
        }
    }
}
