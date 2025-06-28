using FluentAssertions;
using Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class DocumentAssertionsExtensions
    {
        public static void AssertDocument(this Document document , CreateDocumentCommand command ,
            string userId , string customerApplicationId , bool isActive ,DocumentStatus status)
        {
            document.CustomerApplicationId.Should().Be(customerApplicationId);
            document.Type.Should().Be(command.Type);
            document.IssuingCountry.Should().Be(command.IssuingCountry);

        }

        public static void AssertDocumentDto(this DocumentDto dto , Document document)
        {
            dto.Id.Should().Be(document.Id);
            dto.CustomerApplicationId.Should().Be(document.CustomerApplicationId);
            dto.KYCExternalId.Should().Be(document.KYCExternalId);
            dto.Type.Should().Be(document.Type);
            dto.IssuingCountry.Should().Be(document.IssuingCountry);

        }

        
        public static void AssertDocumentAttachmentDto(this DocumentAttachementDto dto , DocumentAttachment attachment)
        {
            dto.Id.Should().Be(attachment.Id);
            dto.KYCExternalId.Should().Be(attachment.Id);
            dto.Side.Should().Be(attachment.Side);
            dto.Size.Should().Be(attachment.Size);
            dto.FileName.Should().Be(attachment.FileName);
        }
    }
}
