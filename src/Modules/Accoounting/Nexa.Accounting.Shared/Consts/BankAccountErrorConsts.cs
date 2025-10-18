using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.Accounting.Shared.Consts
{
    public class BankAccountErrorConsts
    {
        public static NexaError BankAccountNotExist
            => new(nameof(BankAccountNotExist).ToCamelCase(), "Bank account is not exist.");

        public static NexaError BankAccountNotOwned
            => new(nameof(BankAccountNotOwned).ToCamelCase(), "The bank account does not belong to the current user.");
    }
}
