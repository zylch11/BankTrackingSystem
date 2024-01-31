namespace BankTrackingSystem.Models
{
    public class ApplicantMessagesModel
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public string? message { get; set; }
    }
}
