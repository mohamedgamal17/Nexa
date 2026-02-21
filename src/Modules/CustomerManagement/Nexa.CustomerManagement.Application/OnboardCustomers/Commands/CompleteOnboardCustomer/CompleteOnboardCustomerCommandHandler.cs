using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CompleteOnboardCustomer
{
    public class CompleteOnboardCustomerCommandHandler : IApplicationRequestHandler<CompleteOnboardCustomerCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<OnboardCustomer> _onboardCustomerRepository;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public CompleteOnboardCustomerCommandHandler(ICustomerManagementRepository<OnboardCustomer> onboardCustomerRepository, ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory)
        {
            _onboardCustomerRepository = onboardCustomerRepository;
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(CompleteOnboardCustomerCommand request, CancellationToken cancellationToken)
        {
            var onboardCustomer = await _onboardCustomerRepository.SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if(onboardCustomer == null)
            {
                return new Result<CustomerDto>(new EntityNotFoundException(OnboardCustomerErrorConsts.OnboardCustomerNotExist));
            }

            if (onboardCustomer.IsCompleted)
            {
                return new Result<CustomerDto>(new BusinessLogicException(OnboardCustomerErrorConsts.OnboardCustomerCompleted));
            }

            if (!onboardCustomer.HasFullData)
            {
                return new Result<CustomerDto>(new BusinessLogicException(OnboardCustomerErrorConsts.OnboardCustomerIncomplete));
            }


            onboardCustomer.MarkAsCompleted();

            await _onboardCustomerRepository.UpdateAsync(onboardCustomer);

            var customer = await _customerRepository.SingleAsync(x => x.UserId == onboardCustomer.UserId);

            return await _customerResponseFactory.PrepareDto(customer);
        }
    }
}
