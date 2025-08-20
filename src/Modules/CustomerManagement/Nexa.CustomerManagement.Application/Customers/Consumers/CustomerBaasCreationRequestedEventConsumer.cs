using MassTransit;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Events;
using Nexa.Integrations.Baas.Abstractions.Contracts.Clients;
using Nexa.Integrations.Baas.Abstractions.Services;
namespace Nexa.CustomerManagement.Application.Customers.Consumers
{
    public class CustomerBaasCreationRequestedEventConsumer : IConsumer<CustomerBaasCreationRequestedEvent>
    {
        private readonly IBaasClientService _baasProvider;
        private readonly IKYCProvider _kycPrvoider;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public CustomerBaasCreationRequestedEventConsumer(IBaasClientService baasProvider, IKYCProvider kycPrvoider, ICustomerManagementRepository<Customer> customerRepository)
        {
            _baasProvider = baasProvider;
            _kycPrvoider = kycPrvoider;
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<CustomerBaasCreationRequestedEvent> context)
        {
            
            var customer = await _customerRepository.SingleAsync(x => x.Id == context.Message.CustomerId);

            var baasClientRequest = PrepareBaasClientRequest(customer);

            var baasClient = await _baasProvider.CreateClientAsync(baasClientRequest);

            var customerDocument = customer.Document!;

            var baasDocumentRequest = new UploadDocumentRequest
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

        private CreateBaasClientRequest PrepareBaasClientRequest(Customer customer)
        {
            var baasClientRequest = new CreateBaasClientRequest
            {
                Email = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                FirstName = customer.Info!.FirstName,
                LastName = customer.Info.LastName,
                SSN = customer.Info.IdNumber,
                Gender = customer.Info.Gender == Shared.Enums.Gender.Male ? Gender.Male : Gender.Female,
                DateOfBirth = customer.Info.BirthDate,
            };

            if(customer.Info.Address != null)
            {
                baasClientRequest.Address = new Integrations.Baas.Abstractions.Contracts.Clients.Address
                {
                   Country  = customer.Info.Address.Country,
                   City = customer.Info.Address.City,
                   State = customer.Info.Address.State,
                   StreetLine = customer.Info.Address.StreetLine,
                   PostalCode = customer.Info.Address.PostalCode,
                   ZipCode = customer.Info.Address.ZipCode
                };
            }

            return baasClientRequest;
        }
    }
}
