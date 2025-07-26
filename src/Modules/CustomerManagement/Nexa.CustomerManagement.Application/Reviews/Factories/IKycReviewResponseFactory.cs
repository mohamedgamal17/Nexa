using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Reviews.Factories
{
    public interface  IKycReviewResponseFactory : IResponseFactory<KycReview, KycReviewDto>
    {
    }
}
