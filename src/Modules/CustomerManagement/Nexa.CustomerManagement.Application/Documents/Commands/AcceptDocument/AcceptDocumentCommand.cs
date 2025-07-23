using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Documents.Commands.AcceptDocument
{
    public class AcceptDocumentCommand : ICommand
    {
        public string KycCustomeId { get; set; }

        public string KycDocumentId { get; set; }
    }
}
