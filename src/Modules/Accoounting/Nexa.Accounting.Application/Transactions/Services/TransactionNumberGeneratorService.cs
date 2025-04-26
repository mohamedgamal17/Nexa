namespace Nexa.Accounting.Application.Transactions.Services
{
    public class TransactionNumberGeneratorService : ITransactionNumberGeneratorService
    {
        public string Generate()
        {
            return Ulid.NewUlid().ToString();
        }
    }
}
