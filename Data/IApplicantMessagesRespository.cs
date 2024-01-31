using BankTrackingSystem.Models;

namespace BankTrackingSystem.Data
{
    public interface IApplicantMessagesRespository
    {
        Task<List<ApplicantMessagesModel>> GetAllMessagesAsync();
        Task<List<ApplicantMessagesModel>> GetAllAgainstApplicantId(int applicantId);
        Task<ApplicantMessagesModel?> GetMessageAgainstIdAsync(int messageId);
        Task<ApplicantMessagesModel> AddMessage(ApplicantMessagesModel message);
    }
}
