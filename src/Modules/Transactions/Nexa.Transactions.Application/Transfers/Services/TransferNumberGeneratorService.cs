namespace Nexa.Transactions.Application.Transfers.Services
{
    public class TransferNumberGeneratorService : ITransferNumberGeneratorService
    {
        public string Generate()
        {
            return Ulid.NewUlid().ToString();
        }
    }
}
