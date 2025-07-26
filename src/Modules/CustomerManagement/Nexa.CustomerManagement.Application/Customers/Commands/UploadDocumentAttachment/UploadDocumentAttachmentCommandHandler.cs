using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment
{
    public class UploadDocumentAttachmentCommandHandler : IApplicationRequestHandler<UploadDocumentAttachmentCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;

        public UploadDocumentAttachmentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
        }

        public async Task<Result<CustomerDto>> Handle(UploadDocumentAttachmentCommand request, CancellationToken cancellationToken)
        {

            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository
                .AsQuerable()
                .Include(x=> x.Info)
                .Include(x => x.Document)
                .ThenInclude(x => x.Attachments)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Current user customer is not exist."));
            }

            if (customer.Info == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Current user must complete customer info first."));
            }

            if (customer.Document == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Current user must create document first."));

            }

            var document = customer.Document;

            string extensions = request.Data.FileName.Split(".")[1];

            string fileName = $"{DateTime.Now.Ticks}.{extensions}";

            var attachmentRequest = PrepareKYCDocumentAttachement(fileName, request);

            var kycResponse = await _kycProvider.UploadDocumentAttachementAsync(document.KycDocumentId!, attachmentRequest);

            var attachment = new DocumentAttachment(kycResponse.Id, fileName, kycResponse.Size, kycResponse.ContentType, request.Side);

            document.AddAttachment(attachment);

            await _customerRepository.UpdateAsync(customer);

            return await _customerResponseFactory.PrepareDto(customer);
        }

        private KYCDocumentAttachmentRequest PrepareKYCDocumentAttachement(string fileName, UploadDocumentAttachmentCommand command)
        {
            var imageStream = new MemoryStream();

            command.Data.CopyTo(imageStream);

            imageStream.Seek(0, SeekOrigin.Begin);

            var request = new KYCDocumentAttachmentRequest
            {
                FileName = fileName,
                Data = imageStream,
                Side = command.Side
            };

            return request;
        }
    }
}

