namespace Portfolio.ContactForm.Models
{
    public class EmailMessageRequest
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
