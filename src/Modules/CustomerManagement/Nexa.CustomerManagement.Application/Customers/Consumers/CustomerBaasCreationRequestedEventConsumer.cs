using MassTransit;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Baas;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.CustomerManagement.Application.Customers.Consumers
{
    public class CustomerBaasCreationRequestedEventConsumer : IConsumer<CustomerBaasCreationRequestedEvent>
    {
        private readonly IBassProvider _baasProvider;
        private readonly IKYCProvider _kycPrvoider;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public CustomerBaasCreationRequestedEventConsumer(IBassProvider baasProvider, ICustomerManagementRepository<Customer> customerRepository, IKYCProvider kycPrvoider)
        {
            _baasProvider = baasProvider;
            _customerRepository = customerRepository;
            _kycPrvoider = kycPrvoider;
        }

        public async Task Consume(ConsumeContext<CustomerBaasCreationRequestedEvent> context)
        {
            
            var customer = await _customerRepository.SingleAsync(x => x.Id == context.Message.CustomerId);

            var baasClientRequest = new CreateBaasClient
            {
                Email = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                FirstName = customer.Info!.FirstName,
                LastName = customer.Info.LastName,
                SSN = customer.Info.IdNumber,
                Gender = customer.Info.Gender,
                DateOfBirth = customer.Info.BirthDate,
                Address = customer.Info.Address
            };

            var baasClient = await _baasProvider.CreateClientAsync(baasClientRequest);

            var customerDocument = customer.Document!;

            var baasDocumentRequest = new UploadBaasDocument
            {
                Front = await _kycPrvoider
                .DowloadDocumentAttachmentAsync(customerDocument.KycDocumentId, customerDocument.Front!.KycAttachmentId),

                Back = customerDocument.RequireBothSides()
                ? await _kycPrvoider
                    .DowloadDocumentAttachmentAsync(customerDocument.KycDocumentId, customerDocument.Back!.KycAttachmentId)
                : null
            };


            await _baasProvider.UploadDocument(baasClient.Id, baasDocumentRequest);

            customer.Review(baasClient.Id);

            await _customerRepository.UpdateAsync(customer);
        }
    }
}
