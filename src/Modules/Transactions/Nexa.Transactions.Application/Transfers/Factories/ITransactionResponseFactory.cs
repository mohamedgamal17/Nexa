using Nexa.BuildingBlocks.Application.Factories;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Transfers.Factories
{
    public interface ITransactionResponseFactory : IResponseFactory<TransferView, TransferDto>
    {
    }
}
