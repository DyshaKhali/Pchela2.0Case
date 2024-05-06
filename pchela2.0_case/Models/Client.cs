namespace pchela2._0_case.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string NameClient { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public string CustomerCategory { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime LastVisitDate { get; set; }
        public string[] CommunicationMethods { get; set; }
        public string[] AdditionalContacts { get; set; }
        public string CustomerPreferences { get; set; }
        public string LanguageSpeak { get; set; }
        public string MaritalStatus { get; set; }
        public string Notes { get; set; }
        public decimal SatisfactionLevel { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string CustomFields { get; set; }
    }
}
