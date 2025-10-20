using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Reviews.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview
{
    public class CreateKycReviewCommandHandler : IApplicationRequestHandler<CreateKycReviewCommand, KycReviewDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<KycReview> _kycReviewRepository;
        private readonly IKYCProvider _kycProvider;
        private readonly ISecurityContext _securityContext;
        private readonly IKycReviewResponseFactory _kycReviewResponseFactory;

        public CreateKycReviewCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<KycReview> kycReview, IKYCProvider kycProvider, ISecurityContext securityContext, IKycReviewResponseFactory kycReviewResponseFactory)
        {
            _customerRepository = customerRepository;
            _kycReviewRepository = kycReview;
            _kycProvider = kycProvider;
            _securityContext = securityContext;
            _kycReviewResponseFactory = kycReviewResponseFactory;
        }

        public async Task<Result<KycReviewDto>> Handle(CreateKycReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist);
            }

            var kycReviewResult = await CreateDocumentKycReview(customer, request);

            if (kycReviewResult.IsFailure)
            {
                return new Result<KycReviewDto>(kycReviewResult.Exception!);
            }

            await _customerRepository.UpdateAsync(customer);

            return await _kycReviewResponseFactory.PrepareDto(kycReviewResult.Value!);
        }


        private async Task<Result<KycReview>> CreateDocumentKycReview(Customer customer, CreateKycReviewCommand request)
        {
            if (customer.Document == null)
            {
                return new BusinessLogicException(CustomerErrorConsts.DocumentNotExist);
            }

            if (!customer.Document.HasRequireAttachments)
            {
                return new BusinessLogicException(CustomerErrorConsts.IncompleteDocument);
            }

            if (!customer.Document.HasValidStateToBeVerified)
            {
                return new BusinessLogicException(CustomerErrorConsts.InvalidDocumentVerificationState);
            }

            var kycCheck = await CreateKycCheck(customer, request);

            var kycReview = KycReview.Document(customer.Id, kycCheck.Id, request.KycLiveVideoId!);

            customer.ReviewDocument(kycReview);

            return await _kycReviewRepository.InsertAsync(kycReview);
        }
        private async Task<KYCCheck> CreateKycCheck(Customer customer, CreateKycReviewCommand command)
        {
            var kycRequest = new KYCCheckRequest
            {
                ClientId = customer.KycCustomerId!,
                Type = KYCCheckType.DocumentCheck,
                DocumentId = customer.Document!.KycDocumentId,
                LiveVideoId = command.KycLiveVideoId
            };


            return await _kycProvider.CreateCheckAsync(kycRequest);
        }
    }
}
