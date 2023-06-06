using azure_storage_account.Models;

namespace azure_storage_account.Services
{
    public interface IQueueService
    {
        Task SendMessage(EmailMessage emailMessage);
    }
}