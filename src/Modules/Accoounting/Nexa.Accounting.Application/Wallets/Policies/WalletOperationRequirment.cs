namespace Nexa.Accounting.Application.Wallets.Policies
{
    public class WalletOperationRequirment
    {
        public static IsWalletOwnerAuthorizationRequirment IsOwner = new IsWalletOwnerAuthorizationRequirment();
    }
}
