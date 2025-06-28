using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.CustomerApplications
{
    public class CustomerApplication : AggregateRoot
    {
        public string CustomerId { get; set; }
        public string KycExternalId { get;  set; }
        public string? KycCheckId { get;  set; }
        public string? CustomerApplicationExternalId { get;  set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public Gender Gender { get; set; }
        public CustomerApplicationStatus Status { get;private set; }
        public List<Document> Documents { get; set; } = new List<Document>();

        public void AddDocument(Document document)
        {
            Documents.Add(document);
        }

        public Document? FindDocument(string id)
        {
            return Documents.SingleOrDefault(x => x.Id == id);
        }

        public void RemoveDocument(Document document)
        {
            Documents.Remove(document);
        }
    }
}
