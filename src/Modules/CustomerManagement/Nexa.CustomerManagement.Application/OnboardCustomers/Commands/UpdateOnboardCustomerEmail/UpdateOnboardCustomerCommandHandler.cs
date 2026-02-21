using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.OnboardCustomers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerEmail
{
    public class UpdateOnboardCustomerCommandHandler : IApplicationRequestHandler<UpdateOnboardCustomerEmailCommand, OnboardCustomerDto>
    {
        private readonly ICustomerManagementRepository<OnboardCustomer> _onboardCustomerRepository;
        private readonly IOnboardCustomerResponseFactory _onboardCustomerResponseFactory;

        public UpdateOnboardCustomerCommandHandler(ICustomerManagementRepository<OnboardCustomer> onboardCustomerRepository, IOnboardCustomerResponseFactory onboardCustomerResponseFactory)
        {
            _onboardCustomerRepository = onboardCustomerRepository;
            _onboardCustomerResponseFactory = onboardCustomerResponseFactory;
        }

        public async Task<Result<OnboardCustomerDto>> Handle(UpdateOnboardCustomerEmailCommand request, CancellationToken cancellationToken)
        {
            var onboardCustomer = await _onboardCustomerRepository.SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if(onboardCustomer == null)
            {
                return new Result<OnboardCustomerDto>(new EntityNotFoundException(OnboardCustomerErrorConsts.OnboardCustomerNotExist));
            }

            if (onboardCustomer.IsCompleted)
            {
                return new Result<OnboardCustomerDto>(new BusinessLogicException(OnboardCustomerErrorConsts.OnboardCustomerCompleted));
            }

            onboardCustomer.UpdateEmailAddress(request.EmailAddress);

            await _onboardCustomerRepository.UpdateAsync(onboardCustomer);

            return await _onboardCustomerResponseFactory.PrepareDto(onboardCustomer);
        }
    }
}
