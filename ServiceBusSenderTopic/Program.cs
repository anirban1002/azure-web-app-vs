using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://az-servicebus-learning-standard.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jxPSaxwUW0hqtO5buOMKI4/tF4VaMCkOa+ASbOa1CRs=";
const string topicName = "az-servicebus-learning-topic";
const int maxNumberOfMessages = 3;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(serviceBusConnectionString);
sender = client.CreateSender(topicName);

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