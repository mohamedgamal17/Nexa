using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.OnboardCustomers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CreateOnboardCustomer
{
    public class CreateOnboardCustomerCommandHandler : IApplicationRequestHandler<CreateOnboardCustomerCommand, OnboardCustomerDto>
    {
        private readonly ICustomerManagementRepository<OnboardCustomer> _onboardCustomerRepository;

        private readonly IOnboardCustomerResponseFactory _onboardCustomerResponseFactory;
        public CreateOnboardCustomerCommandHandler(ICustomerManagementRepository<OnboardCustomer> onboardCustomerRepository, IOnboardCustomerResponseFactory onboardCustomerResponseFactory)
        {
            _onboardCustomerRepository = onboardCustomerRepository;
            _onboardCustomerResponseFactory = onboardCustomerResponseFactory;
        }

        public async Task<Result<OnboardCustomerDto>> Handle(CreateOnboardCustomerCommand request, CancellationToken cancellationToken)
        {
            var isonboardCustomerExist = await _onboardCustomerRepository.AnyAsync(x => x.UserId == request.UserId);

            if (isonboardCustomerExist)
            {
                return new Result<OnboardCustomerDto>(new ConflictException(OnboardCustomerErrorConsts.OnboardCustomerCreateConflict));
            }

            var onboardCustomer = new OnboardCustomer(request.UserId);

            await _onboardCustomerRepository.InsertAsync(onboardCustomer);

            return await _onboardCustomerResponseFactory.PrepareDto(onboardCustomer);
        }
    }
}
