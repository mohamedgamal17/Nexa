using FluentAssertions;
using Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class DocumentAssertionsExtensions
    {
        public static void AssertDocument(this Document document , CreateDocumentCommand command , string customerId)
        {
            document.Type.Should().Be(command.Type);
            document.KYCExternalId.Should().NotBeNull();
            document.CustomerId.Should().Be(customerId);
        }

        public static void AssertDocumentDto(this DocumentDto dto , Document document)
        {
            dto.Id.Should().Be(document.Id);
            dto.CustomerId.Should().Be(document.CustomerId);
            dto.KYCExternalId.Should().Be(document.KYCExternalId);
            dto.Type.Should().Be(document.Type);

            if(document.Attachments != null)
            {
                dto.Attachements.Should().NotBeNull();

                var attachmentsDtos  = dto.Attachements.OrderBy(x => x.Id).ToList();
                var attachment = document.Attachments.OrderBy(x => x.Id).ToList();


                foreach (var item in Enumerable.Zip<DocumentAttachementDto, DocumentAttachment>(attachmentsDtos, attachment))
                {
                    item.First.AssertDocumentAttachmentDto(item.Second);
                }
                
            }
        }

        
        public static void AssertDocumentAttachmentDto(this DocumentAttachementDto dto , DocumentAttachment attachment)
        {
            dto.Id.Should().Be(attachment.Id);
            dto.KYCExternalId.Should().Be(attachment.KYCExternalId);
            dto.Side.Should().Be(attachment.Side);
            dto.Size.Should().Be(attachment.Size);
            dto.FileName.Should().Be(attachment.FileName);
        }
    }
}
