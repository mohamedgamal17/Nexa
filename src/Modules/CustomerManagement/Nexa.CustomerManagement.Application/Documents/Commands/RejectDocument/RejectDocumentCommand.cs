using Nexa.BuildingBlocks.Application.Requests;
namespace Nexa.CustomerManagement.Application.Documents.Commands.RejectDocument
{
    public class RejectDocumentCommand : ICommand
    {
        public string KycCustomerId { get; set; }
        public string KycDocumentId { get; set; }
    }
}
