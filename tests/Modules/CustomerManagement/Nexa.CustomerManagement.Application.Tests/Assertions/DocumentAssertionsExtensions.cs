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
            string userId , string customerId , bool isActive ,DocumentStatus status)
        {
            document.UserId.Should().Be(userId);
            document.CustomerApplicationId.Should().Be(customerId);
            document.Type.Should().Be(command.Type);
            document.IssuingCountry.Should().Be(command.IssuingCountry);
            document.IsActive.Should().Be(isActive);
            document.Status.Should().Be(status);
        }

        public static void AssertDocumentDto(this DocumentDto dto , Document document)
        {
            dto.Id.Should().Be(document.Id);
            dto.CustomerId.Should().Be(document.CustomerApplicationId);
            dto.UserId.Should().Be(document.UserId);
            dto.KYCExternalId.Should().Be(document.KYCExternalId);
            dto.Type.Should().Be(document.Type);
            dto.IssuingCountry.Should().Be(document.IssuingCountry);
            dto.VerifiedAt.Should().Be(document.VerifiedAt);
            dto.RejectedAt.Should().Be(document.RejectedAt);
            dto.Status.Should().Be(document.Status);
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
