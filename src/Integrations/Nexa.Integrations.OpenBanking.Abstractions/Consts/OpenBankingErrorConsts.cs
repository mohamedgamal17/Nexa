using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.Integrations.OpenBanking.Abstractions.Consts
{
    public class OpenBankingErrorConsts
    {
        public static NexaError BankTokenNotExist
            => new(nameof(BankTokenNotExist).ToCamelCase(), "The specified bank token does not exist.");
        public static NexaError InvalidBankToken
            => new(nameof(InvalidBankToken).ToCamelCase(), "The provided bank token is invalid or has expired.");

        public static NexaError IncompleteBankToken
            => new(nameof(InvalidBankToken).ToCamelCase(), "The provided bank token is completed.");
    }
}
