using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerAddressByUserId
{
    public class UpdateCustomerAddressByUserIdCommandHandler : IApplicationRequestHandler<UpdateCustomerAddressByUserIdCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public UpdateCustomerAddressByUserIdCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory)
        {
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerAddressByUserIdCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if(customer == null)
            {
                return new Result<CustomerDto>(new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist));
            }

            var address = Address.Create(request.Addres.Country, request.Addres.City, request.Addres.State, request.Addres.StreetLine, request.Addres.PostalCode, request.Addres.ZipCode);

            customer.UpdateAddress(address);

            await _customerRepository.UpdateAsync(customer);

            return await _customerResponseFactory.PrepareDto(customer);
        }
    }
}
