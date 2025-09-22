using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Reviews.Queries.ListUserReviews
{
    [Authorize]
    public class ListUserReviewsQuery : PagingParams , IQuery<Paging<KycReviewDto>>
    {

    }
}
