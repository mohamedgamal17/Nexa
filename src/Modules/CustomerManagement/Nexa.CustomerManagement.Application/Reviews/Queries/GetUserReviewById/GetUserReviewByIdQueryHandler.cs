using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Reviews.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Reviews.Queries.GetUserReviewById
{
    public class GetUserReviewByIdQueryHandler : IApplicationRequestHandler<GetUserReviewByIdQuery, KycReviewDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<KycReview> _kycReviewRepository;
        private readonly IKycReviewResponseFactory _kycReviewResponseFactory;
        private readonly ISecurityContext _securityContext;

        public GetUserReviewByIdQueryHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<KycReview> kycReviewRepository, IKycReviewResponseFactory kycReviewResponseFactory, ISecurityContext securityContext)
        {
            _customerRepository = customerRepository;
            _kycReviewRepository = kycReviewRepository;
            _kycReviewResponseFactory = kycReviewResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<KycReviewDto>> Handle(GetUserReviewByIdQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<KycReviewDto>(new BusinessLogicException("current user must complete customer data first."));
            }

            var kycReview = await _kycReviewRepository
                .SingleOrDefaultAsync(
                    x=> x.Id == request.KycReviewId && 
                        x.CustomerId == customer.Id
                 );

            if(kycReview == null)
            {
                return new Result<KycReviewDto>(new EntityNotFoundException(typeof(KycReview), request.KycReviewId));
            }

            return await _kycReviewResponseFactory.PrepareDto(kycReview);

        }
    }
}
