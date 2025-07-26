using FluentAssertions;
using Nexa.CustomerManagement.Application.Reviews.Commands;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class KycReviewAssertionExtensions
    {

        public static void AssertKycReview(this KycReview kycReview ,CreateKycReviewCommand command , Customer customer)
        {
            kycReview.KycCheckId.Should().NotBeNull();
            kycReview.CustomerId.Should().Be(customer.Id);
            kycReview.Type.Should().Be(command.Type);

            if(command.Type == KycReviewType.Document)
            {
                kycReview.KycLiveVideoId.Should().Be(command.KycLiveVideoId);
            }
        }

        public static void AssertKycReviewDto(this KycReviewDto dto , KycReview kycReview)
        {
            dto.Id.Should().Be(kycReview.Id);
            dto.CustomerId.Should().Be(kycReview.CustomerId);
            dto.KycCheckId.Should().Be(kycReview.KycCheckId);
            dto.KycLiveVideoId.Should().Be(kycReview.KycLiveVideoId);
            dto.Type.Should().Be(kycReview.Type);
            dto.Status.Should().Be(kycReview.Status);
            dto.Outcome.Should().Be(kycReview.Outcome);
        }
    }
}
