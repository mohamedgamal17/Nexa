using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.Accounting.Shared.Consts
{
    public class WalletErrorConsts
    {

        public static NexaError WalletNotExist =>
            new (nameof(WalletNotExist).ToCamelCase(), "Current wallet is not exist.");
        public static NexaError WalletNotOwned =>
            new(nameof(WalletNotOwned).ToCamelCase(), "The wallet does not belong to the current user.");
        public static NexaError WalletFrozen
            => new(nameof(WalletFrozen).ToCamelCase(), "Wallet is frozen operation cannot be done.");

        public static NexaError SenderWalletFrozen =>
            new (nameof(SenderWalletFrozen).ToCamelCase(), "Sender wallet is frozen operation cannot be done.");

        public static NexaError ReciverWalletFrozen 
            => new(nameof(ReciverWalletFrozen).ToCamelCase(), "Reciver wallet is frozen operation cannot be done.");

        public static NexaError InsufficentBalance
            => new(nameof(InsufficentBalance).ToCamelCase(), "Insufficient balance to complete the transfer.");

    }
}
