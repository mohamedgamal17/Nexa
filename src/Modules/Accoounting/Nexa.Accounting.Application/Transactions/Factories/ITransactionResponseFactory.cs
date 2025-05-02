using Nexa.Accounting.Application.Transactions.Dtos;
using Nexa.Accounting.Domain.Transactions;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Transactions.Factories
{
    public interface ITransactionResponseFactory : IResponseFactory<TransactionView, TransactionDto>
    {
    }
}
