using MediatR;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Reviews.Commands.UpdateKycReview
{
    public class CompleteKycReviewCommandHandler : IApplicationRequestHandler<CompleteKycReviewCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<KycReview> _kycReviewRepository;

        public CompleteKycReviewCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<KycReview> kycReviewRepository)
        {
            _customerRepository = customerRepository;
            _kycReviewRepository = kycReviewRepository;
        }

        public async Task<Result<Unit>> Handle(CompleteKycReviewCommand request, CancellationToken cancellationToken)
        {
            var kycReview = await _kycReviewRepository.SingleAsync(x => x.KycCheckId == request.KycCheckId);
            var customer = await _customerRepository.SingleAsync(x => x.Id == kycReview.CustomerId);

            if(kycReview.Type == KycReviewType.Info)
            {
                CompleteCustomerInfoReview(request, customer);
            }
            else
            {
                CompleteDocumentReview(request, customer);
            }

            kycReview.Complete(request.Outcome);

            await _kycReviewRepository.UpdateAsync(kycReview);

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }

        private void CompleteCustomerInfoReview(CompleteKycReviewCommand request, Customer customer)
        {
            if (request.Outcome == KycReviewOutcome.Clear)
            {
                customer.AcceptCustomerInfo();
            }
            else
            {
                customer.RejectCustomerInfo();
            }
        }

        private void CompleteDocumentReview(CompleteKycReviewCommand request, Customer customer)
        {
            if (request.Outcome == KycReviewOutcome.Clear)
            {
                customer.AcceptDocument();
            }
            else
            {
                customer.RejectDocument();
            }
        }
    }
}
