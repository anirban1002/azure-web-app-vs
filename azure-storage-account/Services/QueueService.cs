using Azure.Storage.Queues;
using azure_storage_account.Models;
using Newtonsoft.Json;

namespace azure_storage_account.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;
        private string queueName = "attendee-emails";

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(EmailMessage emailMessage)
        {
            var queueClient = new QueueClient(_configuration["StorageConnectionString"],
                queueName,
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });
            await queueClient.CreateIfNotExistsAsync();

            var message = JsonConvert.SerializeObject(emailMessage);

            await queueClient.SendMessageAsync(message);
        }
    }
}
