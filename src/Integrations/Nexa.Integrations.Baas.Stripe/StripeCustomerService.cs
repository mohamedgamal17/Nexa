using Nexa.Integrations.Baas.Abstractions.Contracts.Customers;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeCustomerService : IBaasCustomerService
    {
        private readonly CustomerService _customerService;

        public StripeCustomerService()
        {
            _customerService = new CustomerService();
        }

        public async Task<BaasCustomer> CreateCustomerAsync(CreateBaasCustomerRequest request, CancellationToken cancellationToken = default)
        {
            var customerRequest = new CustomerCreateOptions
            {
                Email = request.Email,
                Phone = request.PhoneNumber,
                Name = request.FirstName + " " + request.LastName,
            };

            var customer = await _customerService.CreateAsync(customerRequest);

            return PrepareBaasCustomer(customer);
        }

        public async Task<BaasCustomer> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var customer = await _customerService.GetAsync(customerId);

            return PrepareBaasCustomer(customer);
        }

        public async Task<BaasCustomer> UpdateCustomerAsync(string clientId, UpdateBaasCustomerRequest request, CancellationToken cancellationToken = default)
        {
            var customerRequest = new CustomerUpdateOptions
            {
                Email = request.Email,
                Phone = request.PhoneNumber,
                Name = request.FirstName + " " + request.LastName,
            };

            var customer = await _customerService.UpdateAsync(clientId,customerRequest);

            return PrepareBaasCustomer(customer);
        }

        private BaasCustomer PrepareBaasCustomer(Customer customer)
        {
            var baasCustomer = new BaasCustomer
            {
                Id = customer.Id,
                Email = customer.Email,
                PhoneNumber = customer.Phone,
            };

            if(customer.Name.Contains(" "))
            {
                var splittedName = customer.Name.Split(" ");

                baasCustomer.FirstName = splittedName[0];
                baasCustomer.LastName = splittedName[1];
            }
            else
            {
                baasCustomer.FirstName = customer.Name;
                baasCustomer.LastName = customer.Name;
            }

            return baasCustomer;
        } 
    }
}
