using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Reviews.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Dtos;
using Vogel.BuildingBlocks.EntityFramework.Extensions;

namespace Nexa.CustomerManagement.Application.Reviews.Queries.ListUserReviews
{
    public class ListUserReviewsQueryHandler : IApplicationRequestHandler<ListUserReviewsQuery, Paging<KycReviewDto>>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<KycReview> _kycReviewRepository;
        private readonly IKycReviewResponseFactory _kycReviewResponseFactory;
        private readonly ISecurityContext _securityContext;

        public ListUserReviewsQueryHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<KycReview> kycReviewRepository, IKycReviewResponseFactory kycReviewResponseFactory, ISecurityContext securityContext)
        {
            _customerRepository = customerRepository;
            _kycReviewRepository = kycReviewRepository;
            _kycReviewResponseFactory = kycReviewResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<KycReviewDto>>> Handle(ListUserReviewsQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new Result<Paging<KycReviewDto>>(new BusinessLogicException("current user must complete customer data first."));
            }

            var result = await _kycReviewRepository.AsQuerable()
                .Where(x => x.CustomerId == customer.Id)
                .ToPaged(request.Skip, request.Length);


            var response = await _kycReviewResponseFactory.PreparePagingDto(result);

            return response;
        }
    }
}
