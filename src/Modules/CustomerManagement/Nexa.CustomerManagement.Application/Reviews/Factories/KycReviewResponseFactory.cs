using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Reviews.Factories
{
    public class KycReviewResponseFactory : ResponseFactory<KycReview, KycReviewDto>, IKycReviewResponseFactory
    {
        public override Task<KycReviewDto> PrepareDto(KycReview view)
        {
            var dto = new KycReviewDto
            {
                Id = view.Id,
                CustomerId = view.CustomerId,
                KycLiveVideoId = view.KycLiveVideoId,
                KycCheckId = view.KycCheckId,
                Status = view.Status,
                Outcome = view.Outcome
            };

            return Task.FromResult(dto);
        }
    }
}
