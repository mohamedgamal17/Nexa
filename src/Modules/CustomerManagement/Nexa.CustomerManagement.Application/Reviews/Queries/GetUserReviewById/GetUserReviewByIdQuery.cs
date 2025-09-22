using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Reviews.Queries.GetUserReviewById
{
    [Authorize]
    public class GetUserReviewByIdQuery : IQuery<KycReviewDto>
    {
        public string KycReviewId { get; set; }
    }
}
