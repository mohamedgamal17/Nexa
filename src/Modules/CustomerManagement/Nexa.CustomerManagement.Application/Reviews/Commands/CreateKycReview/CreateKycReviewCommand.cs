using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview
{
    [Authorize]
    public class CreateKycReviewCommand : ICommand<KycReviewDto>
    {
        public KycReviewType Type { get; set; }
        public string? KycLiveVideoId { get; set; }
    }
}
