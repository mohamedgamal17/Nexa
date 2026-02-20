using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.OnboardCustomers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.OnboardCustomers.Queries.GetOnboardCustomerByUserId
{
    public class GetOnboardCustomerByUserIdQueryHandler : IApplicationRequestHandler<GetOnboardCustomerByUserIdQuery, OnboardCustomerDto>
    {
        private readonly ICustomerManagementRepository<OnboardCustomer> _onboardCustomerRepository;
        private readonly IOnboardCustomerResponseFactory _onboardCustomerResponseFactory;

        public GetOnboardCustomerByUserIdQueryHandler(ICustomerManagementRepository<OnboardCustomer> onboardCustomerRepository, IOnboardCustomerResponseFactory onboardCustomerResponseFactory)
        {
            _onboardCustomerRepository = onboardCustomerRepository;
            _onboardCustomerResponseFactory = onboardCustomerResponseFactory;
        }

        public async Task<Result<OnboardCustomerDto>> Handle(GetOnboardCustomerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var query = _onboardCustomerRepository.AsQuerable();

            var onboardCustomer = await query.SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if(onboardCustomer == null)
            {
                return new Result<OnboardCustomerDto>(new EntityNotFoundException(OnboardCustomerErrorConsts.OnboardCustomerNotExist));
            }

            return await _onboardCustomerResponseFactory.PrepareDto(onboardCustomer);
        }
    }

}
