using Nexa.BuildingBlocks.Application.Factories;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Dtos;

namespace Nexa.Transactions.Application.Transfers.Factories
{
    public interface ITransactionResponseFactory : IResponseFactory<TransferView, TransferDto>
    {
    }
}
