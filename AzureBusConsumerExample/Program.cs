

using System.Text.Json;
using System.Text;
using Microsoft.Azure.ServiceBus;


AzureBusEx.Go();
Console.ReadLine();

public class AzureBusEx
{
    const string connString = "Endpoint=sb://testservicebusex.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ACk/H1ePL7OBhEeomkKHeqAHfIqi3owkoISNYYYV6b4=";
    static IQueueClient qClient;
    private static async Task ProcessMessageAsync(Message msg, CancellationToken token)
    {
        // Deserialisr the msg body
        var jsonBody = Encoding.UTF8.GetString(msg.Body);
        var personObj = JsonSerializer.Deserialize<AzureBusExample.Models.Person>(jsonBody);

        Console.WriteLine($"First Name: {personObj.FirstName}");
        Console.WriteLine($"Last Name: {personObj.LastName}");
        Console.WriteLine($"Email: {personObj.Email}");

        // Updating the queue that the message has been processed successfully
        await qClient.CompleteAsync(msg.SystemProperties.LockToken);
    }

    private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        Console.WriteLine($"Something went wrong, {args.Exception}");
        return Task.CompletedTask;
    }
    public static async Task Go()
    {
        qClient = new QueueClient(connString, "mynewqueue");

        var msgOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            // How many messages we can process at time
            MaxConcurrentCalls = 1,
            // need to wait until a message is fully processed
            AutoComplete = false,
        };

        qClient.RegisterMessageHandler(ProcessMessageAsync, msgOptions);

        Console.ReadLine();
        await qClient.CloseAsync();
    }
}