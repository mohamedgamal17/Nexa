namespace Nexa.Transactions.Domain.Transfers
{
    public class TransferTableConsts
    {
        public static string TableName = "Transfers"; 

        public static int IdLength = 256;

        public static int UserIdLength = 256;

        public static int WalletIdLength = 256;

        public static int ExternalTransferIdLength = 256;

        public static int NumberLength = 256;

        public static int CounterPartyIdLength = 256;

        public static int ReciverIdLength = 256;

        public static string Type = nameof(Type);

    }
}
