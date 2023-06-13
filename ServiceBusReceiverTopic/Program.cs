using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://az-servicebus-learning-standard.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jxPSaxwUW0hqtO5buOMKI4/tF4VaMCkOa+ASbOa1CRs=";
const string topicName = "az-servicebus-learning-topic";
const string subscriptionName = "az-servicebus-learning-topic-subscription1";

ServiceBusClient client;
ServiceBusProcessor processor = default!;

async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    string body = processMessageEventArgs.Message.Body.ToString();
    Console.WriteLine($"{body} - Subscription: {subscriptionName}");
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processErrorEventArgs)
{
    Console.WriteLine(processErrorEventArgs.Exception.ToString());
    return Task.CompletedTask;
}

client = new ServiceBusClient(serviceBusConnectionString);
processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end processing");
    Console.ReadLine();

    Console.WriteLine("\nStopping the receiver....");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving message");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}