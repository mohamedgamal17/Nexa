using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.Transactions.Shared.Consts
{
    public class TransferErrorConsts
    {
        public static NexaError TransferNotExist
            => new(nameof(TransferNotExist).ToCamelCase(), "Transfer is not exist.");
    }
}
