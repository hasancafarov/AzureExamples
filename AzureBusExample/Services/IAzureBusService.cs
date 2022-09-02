using AzureBusExample.Models;
using System;

namespace AzureBusExample.Services
{
    public interface IAzureBusService
    {
        Task SendMessageAsync(Person personMessage, string queueName);

    }
}
