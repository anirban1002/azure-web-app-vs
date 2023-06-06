using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://az-servicebus-learning.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lR+XlS6UjM1MEQqsRB0MbD+v6XVW5ZsuR+ASbAmczm0=";
const string queueName = "az-servicebus-learning-queue1";
const int maxNumberOfMessages = 3;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(serviceBusConnectionString);
sender = client.CreateSender(queueName);

using (ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync())
{
    for (int i = 1; i <= maxNumberOfMessages; i++)
    {
        if (!batch.TryAddMessage(new ServiceBusMessage($"This is a message - {i}")))
        {
            Console.WriteLine($"message - {i} was not added to the batch");
        }
    }

    try
    {
        await sender.SendMessagesAsync(batch);
        Console.WriteLine("Messages Sent");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception:{ex.Message}");
    }
    finally
    {
        await sender.DisposeAsync();
        await client.DisposeAsync();
    }
}

