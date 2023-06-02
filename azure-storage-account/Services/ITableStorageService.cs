using azure_storage_account.Data;

namespace azure_storage_account.Services
{
    public interface ITableStorageService
    {
        Task DeleteAttendee(string industry, string id);
        Task<AttendeeEntity> GetAttendee(string industry, string id);
        Task<List<AttendeeEntity>> GetAttendees();
        Task UpsertAttendee(AttendeeEntity attendeeEntity);
    }
}